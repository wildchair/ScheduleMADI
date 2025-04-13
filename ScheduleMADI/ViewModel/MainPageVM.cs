using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
using ScheduleCore.Models.Madi;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace ScheduleMADI
{
    public class MainPageVM : INotifyPropertyChanged
    {
        public WithoutCarouselVM withoutCarouselVM { get; set; }
        public List<Day> Schedule//принимает семидневное, устанавливает 14дневное
        {
            get => schedule;
            set
            {
                if (value == null)
                {
                    schedule = null;
                    OnPropertyChanged();
                    return;
                }

                List<Day> days1 = new List<Day>()
                {
                new Day(DayOfWeek.Monday, new ObservableCollection<Class>(), "Числитель"),
                new Day(DayOfWeek.Tuesday, new ObservableCollection<Class>(), "Числитель"),
                new Day(DayOfWeek.Wednesday, new ObservableCollection<Class>(), "Числитель"),
                new Day(DayOfWeek.Thursday, new ObservableCollection<Class>(), "Числитель"),
                new Day(DayOfWeek.Friday, new ObservableCollection<Class>(), "Числитель"),
                new Day(DayOfWeek.Saturday, new ObservableCollection<Class>(), "Числитель"),
                new Day(DayOfWeek.Sunday, new ObservableCollection < Class >(), "Числитель")
                };
                List<Day> days2 = new List<Day>()
                {
                new Day(DayOfWeek.Monday, new ObservableCollection<Class>(), "Знаменатель"),
                new Day(DayOfWeek.Tuesday, new ObservableCollection<Class>(), "Знаменатель"),
                new Day(DayOfWeek.Wednesday, new ObservableCollection<Class>(),"Знаменатель"),
                new Day(DayOfWeek.Thursday, new ObservableCollection<Class>(), "Знаменатель"),
                new Day(DayOfWeek.Friday, new ObservableCollection<Class>(), "Знаменатель"),
                new Day(DayOfWeek.Saturday, new ObservableCollection<Class>(), "Знаменатель"),
                new Day(DayOfWeek.Sunday, new ObservableCollection < Class >(), "Знаменатель")
                };

                for (int i = 0; i < 7; i++)//Сборка итогового двухнедельного расписания
                    foreach (var lesson in value[i].Lessons)
                        if (lesson.TypeOfWeek == "Еженедельно")
                        {
                            days1[i].Lessons.Add(new Class
                            {
                                TypeOfWeek = lesson.TypeOfWeek,
                                Name = lesson.Name,
                                Visitors = lesson.Visitors,
                                Classroom = lesson.Classroom,
                                Time = lesson.Time,
                                Type = lesson.Type
                            });
                            days2[i].Lessons.Add(new Class
                            {
                                TypeOfWeek = lesson.TypeOfWeek,
                                Name = lesson.Name,
                                Visitors = lesson.Visitors,
                                Classroom = lesson.Classroom,
                                Time = lesson.Time,
                                Type = lesson.Type
                            });
                        }
                        else if (lesson.TypeOfWeek == "Числитель" || lesson.TypeOfWeek == "Числ. 1 раз в месяц")
                        {
                            days1[i].Lessons.Add(new Class
                            {
                                TypeOfWeek = lesson.TypeOfWeek,
                                Name = lesson.Name,
                                Visitors = lesson.Visitors,
                                Classroom = lesson.Classroom,
                                Time = lesson.Time,
                                Type = lesson.Type
                            });
                        }
                        else
                        {
                            days2[i].Lessons.Add(new Class
                            {
                                TypeOfWeek = lesson.TypeOfWeek,
                                Name = lesson.Name,
                                Visitors = lesson.Visitors,
                                Classroom = lesson.Classroom,
                                Time = lesson.Time,
                                Type = lesson.Type
                            });
                        }

                schedule = days1.Concat(days2).ToList();

                OnPropertyChanged();
            }
        }
        private List<Day> schedule;

        public string GroupLabel
        {
            get => groupLabel;
            set
            {
                if (groupLabel != value)
                {
                    groupLabel = value;
                    OnPropertyChanged();
                }
            }
        }
        private string groupLabel;

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

        public DateTime MinDate { get => minDate; }//минимальная дата для датапикера
        private readonly DateTime minDate = DateTime.Now.AddMonths(-1);
        public DateTime MaxDate { get => maxDate; }//максимальна дата для датапикера
        private readonly DateTime maxDate = DateTime.Now.AddMonths(2);

        public bool Datepicker_is_enabled
        {
            get => datepicker_is_enabled;
            set
            {
                if (datepicker_is_enabled != value)
                {
                    datepicker_is_enabled = value;
                    OnPropertyChanged();
                }
            }
        }
        private bool datepicker_is_enabled = false;

        public event PropertyChangedEventHandler PropertyChanged;

        private readonly TokenDistributor tokenDistributor = new();

        public MainPageVM()
        {
            BufferedMADI.PropertyChanged += OnIdMADIPropertyChanged;

            withoutCarouselVM = new(this);

            LoadFirstData();//тут токен наверное не нужен, пусть грузит себе всё вместе и норм. Вопрос лишь в том,грузить все id или выборочно

            if (BufferedMADI.Id.Value == null)
                EmptyString = "Введите группу. \"Настройки\" -> \"Группа\"";
            else
                GroupLabel = BufferedMADI.Id.Value;
        }

        private async Task<bool> LoadFirstData()//загрузка с нуля
        {
            Datepicker_is_enabled = false;
            bool bufferedLoaded = false;

            while (true)
            {
                EmptyString = "Загрузка групп и недели...";


                var getGroups = ParseMADI.GetGroups(new CancellationToken());
                var getWeek = ParseMADI.GetWeek(new CancellationToken());
                var GetProfessors = ParseMADI.GetProfessors(new CancellationToken());
                try
                {
                    await Task.WhenAll(getGroups, getWeek, GetProfessors);
                    break;
                }
                catch(Exception ex)
                {
                    if (BufferedMADI.BufferedDay.Value != null && !bufferedLoaded
                        && BufferedMADI.Id.Key == BufferedMADI.BufferedSchedule.Key)
                    {
                        WeekMADI.Week = BufferedMADI.BufferedDay.Value;
                        WeekMADI.Week = WeekMADI.WeekByDate(BufferedMADI.BufferedDay.Key, DateTime.Now.Date);
                        Schedule = await ParseMADI.GetScheduleFromHTML(BufferedMADI.BufferedSchedule.Value);

                        Datepicker_is_enabled = true;

                        CancellationTokenSource cancellationToken = new CancellationTokenSource();
                        var toast1 = Toast.Make($"Загружено расписание от {BufferedMADI.BufferedDay.Key.Date.ToShortDateString()}.", ToastDuration.Long);
                        await toast1.Show(cancellationToken.Token);

                        bufferedLoaded = true;
                    }

                    for (int i = 5; i > 0; i--)
                    {
                        EmptyString = $"Не удалось подключиться. Повторная попытка через: {i} секунд...";
                        await Task.Delay(1000);
                    }
                }
            }

            if (BufferedMADI.Id.Value != null)
                return await LoadSecondData(bufferedLoaded, tokenDistributor.GetNewToken());

            EmptyString = "Введите группу. \"Настройки\" -> \"Группа\"";
            return true;
        }

        private async Task<bool> LoadSecondData(bool bufferedLoaded, CancellationToken cancellationToken)//загрузка по группе
        {
            Datepicker_is_enabled = false;

            while (true)
            {
                withoutCarouselVM.TapNums = 0;
                EmptyString = "Загрузка расписания...";

                try
                {
                    Schedule = await ParseMADI.GetSchedule(BufferedMADI.Id, cancellationToken);
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
                    if (BufferedMADI.BufferedSchedule.Value != null &&
                        BufferedMADI.Id.Key == BufferedMADI.BufferedSchedule.Key && !bufferedLoaded)
                    {
                        Schedule = await ParseMADI.GetScheduleFromHTML(BufferedMADI.BufferedSchedule.Value);

                        Datepicker_is_enabled = true;
                        bufferedLoaded = true;
                    }

                    for (int i = 5; i > 0; i--)
                    {
                        EmptyString = $"Не удалось подключиться. Повторная попытка через: {i} секунд...";
                        await Task.Delay(1000);
                    }
                }
            }

            Datepicker_is_enabled = true;

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
                if (Schedule != null)
                {
                    Schedule.Clear();
                    OnPropertyChanged(nameof(Schedule));
                }

                tokenDistributor.CancelActiveToken();

                LoadSecondData(false, tokenDistributor.GetNewToken());
            }
        }
        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        public class TokenDistributor
        {
            private List<CancellationTokenSource> token_storage = new();

            public CancellationToken GetActiveToken()
            {
                return token_storage.Last().Token;
            }
            public CancellationToken GetNewToken()
            {
                token_storage.Add(new CancellationTokenSource());
                return GetActiveToken();
            }
            public async void CancelActiveToken()
            {
                if (token_storage.Count == 0)
                    return;

                var current_token = token_storage.Last();
                current_token.Cancel();

                await Task.Delay(40000);

                token_storage.Remove(current_token);
                current_token.Dispose();
            }
        }
    }
}