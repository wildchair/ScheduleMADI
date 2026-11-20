using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace ScheduleMADI
{
    public class MainPageVM : INotifyPropertyChanged
    {
        public CarouselVM Carousel { get; }
        public List<Day> Schedule//принимает семидневное, устанавливает 14дневное
        {
            get => schedule;
            set
            {
                if (value == null)
                {
                    schedule = null;
                    Carousel.SetSchedule(schedule, DateTime.Today);
                    OnPropertyChanged();
                    return;
                }

                List<Day> days1 = new List<Day>()
                {
                new Day(DayOfWeek.Monday, new ObservableCollection<Lesson>(), "Числитель"),
                new Day(DayOfWeek.Tuesday, new ObservableCollection<Lesson>(), "Числитель"),
                new Day(DayOfWeek.Wednesday, new ObservableCollection<Lesson>(), "Числитель"),
                new Day(DayOfWeek.Thursday, new ObservableCollection<Lesson>(), "Числитель"),
                new Day(DayOfWeek.Friday, new ObservableCollection<Lesson>(), "Числитель"),
                new Day(DayOfWeek.Saturday, new ObservableCollection<Lesson>(), "Числитель"),
                new Day(DayOfWeek.Sunday, new ObservableCollection < Lesson >(), "Числитель")
                };
                List<Day> days2 = new List<Day>()
                {
                new Day(DayOfWeek.Monday, new ObservableCollection<Lesson>(), "Знаменатель"),
                new Day(DayOfWeek.Tuesday, new ObservableCollection<Lesson>(), "Знаменатель"),
                new Day(DayOfWeek.Wednesday, new ObservableCollection<Lesson>(),"Знаменатель"),
                new Day(DayOfWeek.Thursday, new ObservableCollection<Lesson>(), "Знаменатель"),
                new Day(DayOfWeek.Friday, new ObservableCollection<Lesson>(), "Знаменатель"),
                new Day(DayOfWeek.Saturday, new ObservableCollection<Lesson>(), "Знаменатель"),
                new Day(DayOfWeek.Sunday, new ObservableCollection < Lesson >(), "Знаменатель")
                };

                for (int i = 0; i < 7; i++)//Сборка итогового двухнедельного расписания
                    foreach (var lesson in value[i].Lessons)
                        if (lesson.CardDay == "Еженедельно")
                        {
                            days1[i].Lessons.Add(new Lesson
                            {
                                CardDay = lesson.CardDay,
                                CardName = lesson.CardName,
                                CardProf = lesson.CardProf,
                                CardRoom = lesson.CardRoom,
                                CardTime = lesson.CardTime,
                                CardType = lesson.CardType
                            });
                            days2[i].Lessons.Add(new Lesson
                            {
                                CardDay = lesson.CardDay,
                                CardName = lesson.CardName,
                                CardProf = lesson.CardProf,
                                CardRoom = lesson.CardRoom,
                                CardTime = lesson.CardTime,
                                CardType = lesson.CardType
                            });
                        }
                        else if (lesson.CardDay == "Числитель" || lesson.CardDay == "Числ. 1 раз в месяц")
                        {
                            days1[i].Lessons.Add(new Lesson
                            {
                                CardDay = lesson.CardDay,
                                CardName = lesson.CardName,
                                CardProf = lesson.CardProf,
                                CardRoom = lesson.CardRoom,
                                CardTime = lesson.CardTime,
                                CardType = lesson.CardType
                            });
                        }
                        else
                        {
                            days2[i].Lessons.Add(new Lesson
                            {
                                CardDay = lesson.CardDay,
                                CardName = lesson.CardName,
                                CardProf = lesson.CardProf,
                                CardRoom = lesson.CardRoom,
                                CardTime = lesson.CardTime,
                                CardType = lesson.CardType
                            });
                        }

                schedule = days1.Concat(days2).ToList();
                Carousel.SetSchedule(schedule, DateTime.Today);

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
        private bool initialDataLoaded;

        public MainPageVM()
        {
            BufferedMADI.PropertyChanged += OnIdMADIPropertyChanged;
            Carousel = new(MinDate, MaxDate, DateTime.Today);
            GroupLabel = BufferedMADI.Id.Value;

            // Сначала загружаем справочники и учебную неделю, затем расписание выбранной группы.
            _ = LoadFirstData();
        }

        private async Task LoadFirstData()
        {
            string cachedGroup = null;

            while (true)
            {
                EmptyString = "Загрузка групп и недели...";
                try
                {
                    await Task.WhenAll(ParseMADI.GetGroups(CancellationToken.None),
                        ParseMADI.GetWeek(CancellationToken.None),
                        ParseMADI.GetProfessors(CancellationToken.None));
                    break;
                }
                catch (Exception error)
                {
                    System.Diagnostics.Debug.WriteLine(error);
                    var requested = BufferedMADI.Id;
                    if ((requested.Key != cachedGroup || Schedule is not { Count: > 0 })
                        && await TryShowCachedSchedule(requested.Key, CancellationToken.None, restoreWeek: true))
                    {
                        cachedGroup = requested.Key;
                        var toast = Toast.Make($"Загружено расписание от {BufferedMADI.BufferedDay.Key.Date.ToShortDateString()}.", ToastDuration.Long);
                        await toast.Show();
                    }

                    await WaitForRetry(CancellationToken.None);
                }
            }

            initialDataLoaded = true;
            if (BufferedMADI.Id.Value != null)
                await LoadSecondData(cachedGroup == BufferedMADI.Id.Key && Schedule is { Count: > 0 },
                    tokenDistributor.GetNewToken());
            else
                EmptyString = "Введите группу. \"Настройки\" -> \"Группа\"";
        }

        private async Task LoadSecondData(bool showingCache, CancellationToken cancellationToken)
        {
            var requested = BufferedMADI.Id;
            Datepicker_is_enabled = showingCache;

            while (!cancellationToken.IsCancellationRequested)
            {
                EmptyString = "Загрузка расписания...";
                try
                {
                    var schedule = await ParseMADI.GetSchedule(requested, cancellationToken);
                    cancellationToken.ThrowIfCancellationRequested();
                    Schedule = schedule;
                    Datepicker_is_enabled = true;
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
                        showingCache = await TryShowCachedSchedule(requested.Key, cancellationToken);

                    if (!await WaitForRetry(cancellationToken))
                        return;
                }
            }

            if (showingCache && !cancellationToken.IsCancellationRequested)
                await Toast.Make("Подключение восстановлено", ToastDuration.Short).Show();
        }

        private async Task<bool> TryShowCachedSchedule(string groupId, CancellationToken cancellationToken, bool restoreWeek = false)
        {
            var cached = BufferedMADI.BufferedSchedule;
            var week = BufferedMADI.BufferedDay;
            if (string.IsNullOrEmpty(groupId) || cached.Key != groupId || string.IsNullOrEmpty(cached.Value)
                || (restoreWeek && (week.Value is not ("Числитель" or "Знаменатель") || week.Key == DateTime.MinValue)))
                return false;

            List<Day> schedule;
            try
            {
                schedule = await ParseMADI.GetScheduleFromHTML(cached.Value);
            }
            catch (Exception error)
            {
                System.Diagnostics.Debug.WriteLine(error);
                return false;
            }

            if (schedule?.Count != 7 || cancellationToken.IsCancellationRequested || groupId != BufferedMADI.Id.Key)
                return false;

            if (restoreWeek)
            {
                WeekMADI.Week = week.Value;
                WeekMADI.Week = WeekMADI.WeekByDate(week.Key.Date, DateTime.Today);
            }
            Schedule = schedule;
            Datepicker_is_enabled = true;
            return true;
        }

        private Task<bool> WaitForRetry(CancellationToken cancellationToken) =>
            WaitForRetry(seconds => EmptyString = $"Не удалось подключиться. Повторная попытка через: {seconds} секунд...",
                cancellationToken);

        internal static async Task<bool> WaitForRetry(Action<int> reportRemaining, CancellationToken cancellationToken)
        {
            try
            {
                for (int seconds = 5; seconds > 0; seconds--)
                {
                    cancellationToken.ThrowIfCancellationRequested();
                    reportRemaining(seconds);
                    await Task.Delay(1000, cancellationToken);
                }
                return !cancellationToken.IsCancellationRequested;
            }
            catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
            {
                return false;
            }
        }

        private void OnIdMADIPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(BufferedMADI.Id))
            {
                GroupLabel = BufferedMADI.Id.Value;
                if (Schedule != null)
                {
                    Schedule.Clear();
                    Carousel.SetSchedule(Schedule, DateTime.Today);
                    OnPropertyChanged(nameof(Schedule));
                }

                Datepicker_is_enabled = false;
                if (initialDataLoaded)
                {
                    tokenDistributor.CancelActiveToken();
                    _ = LoadSecondData(false, tokenDistributor.GetNewToken());
                }
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
