using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
using CommunityToolkit.Maui.Core.Extensions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace ScheduleMADI
{
    public class ExamPageVM : INotifyPropertyChanged
    {
        public string GroupLabel
        {
            get => _groupLabel;
            set
            {
                if (_groupLabel != value)
                {
                    _groupLabel = value;
                    OnPropertyChanged();
                }
            }
        }
        private string _groupLabel;

        public string EmptyString
        {
            get => emptyString;
            set
            {
                if (emptyString != value)
                {
                    emptyString = value;
                    OnPropertyChanged();
                }
            }
        }
        private string emptyString;

        public ObservableCollection<Exam> ExamShedule 
        { 
            get => _examShedule; 
            set
            {
                if (_examShedule != value)
                {
                    _examShedule = value;
                    OnPropertyChanged();
                }
            }
        }
        private ObservableCollection<Exam> _examShedule = new();

        public event PropertyChangedEventHandler PropertyChanged;

        private readonly MainPageVM.TokenDistributor _tokenDistributor = new();

        public ExamPageVM()
        {
            BufferedMADI.PropertyChanged += OnIdMADIPropertyChanged;

            if (BufferedMADI.Id.Value != null)
            {
                _ = LoadSecondData(_tokenDistributor.GetNewToken());
                GroupLabel = BufferedMADI.Id.Value;
            }
            else
                EmptyString = "Введите группу. \"Настройки\" -> \"Группа\"";

        }

        private async Task LoadSecondData(CancellationToken cancellationToken)
        {
            var requested = BufferedMADI.Id;
            bool showingCache = false;

            while (!cancellationToken.IsCancellationRequested)
            {
                EmptyString = "Загрузка экзаменов...";
                try
                {
                    var exams = await ParseMADI.GetExamSchedule(requested, cancellationToken);
                    cancellationToken.ThrowIfCancellationRequested();
                    ExamShedule = exams.ToObservableCollection();
                    EmptyString = string.Empty;
                    break;
                }
                catch (ParseMADIException error)
                {
                    if (!cancellationToken.IsCancellationRequested)
                        EmptyString = error.Message;
                    return;
                }
                catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
                {
                    return;
                }
                catch (Exception error)
                {
                    if (cancellationToken.IsCancellationRequested)
                        return;

                    System.Diagnostics.Debug.WriteLine(error);
                    if (!showingCache)
                        showingCache = await TryShowCachedExams(requested.Key, cancellationToken);

                    if (!await MainPageVM.WaitForRetry(seconds =>
                        EmptyString = $"Не удалось подключиться. Повторная попытка через: {seconds} секунд...", cancellationToken))
                        return;
                }
            }

            if (showingCache && !cancellationToken.IsCancellationRequested)
                await Toast.Make("Подключение восстановлено", ToastDuration.Short).Show();
        }

        private async Task<bool> TryShowCachedExams(string groupId, CancellationToken cancellationToken)
        {
            var cached = BufferedMADI.BufferedExamSchedule;
            if (string.IsNullOrEmpty(groupId) || groupId != cached.Key || string.IsNullOrEmpty(cached.Value))
                return false;

            List<Exam> exams;
            try
            {
                exams = await ParseMADI.GetExamScheduleFromHTML(cached.Value);
            }
            catch (Exception error)
            {
                System.Diagnostics.Debug.WriteLine(error);
                return false;
            }

            if (cancellationToken.IsCancellationRequested || groupId != BufferedMADI.Id.Key)
                return false;

            ExamShedule = exams.ToObservableCollection();
            return true;
        }

        private void OnIdMADIPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(BufferedMADI.Id))
            {
                GroupLabel = BufferedMADI.Id.Value;
                if (ExamShedule != null)
                {
                    ExamShedule.Clear();
                    OnPropertyChanged(nameof(ExamShedule));
                }

                _tokenDistributor.CancelActiveToken();

                _ = LoadSecondData(_tokenDistributor.GetNewToken());
            }
        }

        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
