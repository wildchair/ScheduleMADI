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

        public MainPageVM()
        {
            IdMADI.PropertyChanged += OnIdMADIPropertyChanged;

            withoutCarouselVM = new(this);

            LoadFirstData();

            if (IdMADI.Id.Value == null)
                EmptyString = "Введите группу. \"Настройки\" -> \"Группа\"";
            else
                GroupLabel = IdMADI.Id.Value;
        }

        private async Task<bool> LoadFirstData()
        {
            Datepicker_is_enabled = false;

            while (true)
            {
                EmptyString = "Загрузка групп и недели...";

                var getGroups = ParseMADI.GetGroups();
                var getWeek = ParseMADI.GetWeek();
                try
                {
                    await Task.WhenAll(getGroups, getWeek);
                    break;
                }
                catch
                {
                    for (int i = 10; i > 0; i--)
                    {
                        EmptyString = $"Не удалось подключиться. Повторная попытка через: {i} секунд...";
                        await Task.Delay(1000);
                    }
                }
            }

            if (IdMADI.Id.Value != null)
                return await LoadSecondData();

            return true;
        }

        private async Task<bool> LoadSecondData()
        {
            Datepicker_is_enabled = false;

            while (true)
            {
                withoutCarouselVM.TapNums = 0;
                EmptyString = "Загрузка расписания...";

                try
                {
                    Schedule = await ParseMADI.GetShedule(/*IdMADI.Id.Key*/);
                    break;
                }
                catch (ParseMADIException ex)
                {
                    EmptyString = ex.Message;
                    return false;
                }
                catch
                {
                    for (int i = 10; i > 0; i--)
                    {
                        EmptyString = $"Не удалось подключиться. Повторная попытка через: {i} секунд...";
                        await Task.Delay(1000);
                    }
                }
            }

            Datepicker_is_enabled = true;
            return true;
        }

        private void OnIdMADIPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            GroupLabel = IdMADI.Id.Value;
            if (Schedule != null)
            {

                Schedule.Clear();
                OnPropertyChanged(nameof(Schedule));
            }
            LoadSecondData();
        }
        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
