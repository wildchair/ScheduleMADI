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
                LoadSecondData(false, _tokenDistributor.GetNewToken());
                GroupLabel = BufferedMADI.Id.Value;
            }
            else
                EmptyString = "Введите группу. \"Настройки\" -> \"Группа\"";

        }

        private async Task<bool> LoadSecondData(bool bufferedLoaded, CancellationToken cancellationToken)//загрузка по группе
        {
            while (true)
            {
                EmptyString = "Загрузка экзаменов...";

                try
                {
                    var exams = await ParseMADI.GetExamSchedule(BufferedMADI.Id, cancellationToken);
                    ExamShedule = exams.ToObservableCollection();
                    break;
                }
                catch (ParseMADIException ex)
                {
                    EmptyString = ex.Message;
                    return false;
                }
                catch (OperationCanceledException)
                {
                    //CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
                    //var toast = Toast.Make("Отменено", ToastDuration.Short);
                    //await toast.Show(cancellationTokenSource.Token);
                    return false;
                }
                catch
                {
                    if (BufferedMADI.BufferedExamSchedule.Value != null &&
                        BufferedMADI.Id.Key == BufferedMADI.BufferedExamSchedule.Key && !bufferedLoaded)
                    {
                        var exams = await ParseMADI.GetExamScheduleFromHTML(BufferedMADI.BufferedExamSchedule.Value);
                        ExamShedule = exams.ToObservableCollection();

                        bufferedLoaded = true;
                    }

                    for (int i = 5; i > 0; i--)
                    {
                        EmptyString = $"Не удалось подключиться. Повторная попытка через: {i} секунд...";
                        await Task.Delay(1000);
                    }
                }
            }

            if (bufferedLoaded)
            {
                CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
                var toast = Toast.Make("Подключение восстановлено", ToastDuration.Short);
                await toast.Show(cancellationTokenSource.Token);
            }

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

                LoadSecondData(false, _tokenDistributor.GetNewToken());
            }
        }

        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
