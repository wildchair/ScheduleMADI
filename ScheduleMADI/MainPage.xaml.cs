using System.Collections.ObjectModel;

namespace ScheduleMADI;

public partial class MainPage : ContentPage
{
    private List<Day> shedule;//фулл расписание 

    public ObservableCollection<Day> Days { get => days; set { days = value; OnPropertyChanged(); } }
    private ObservableCollection<Day> days = new();
    public DateTime MinDate { get => minDate; set => minDate = value; }
    private DateTime minDate = DateTime.Now.AddMonths(-1);
    public DateTime MaxDate { get => maxDate; set => maxDate = value; }
    private DateTime maxDate = DateTime.Now.AddMonths(2);

    public string EntryText
    {
        get => entryText;
        set
        {
            if (entryText != value)
            {
                entryText = value;
                OnPropertyChanged();
            }
        }
    }
    private string entryText;

    private DateTime today = DateTime.Now.Date;

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

    public bool Entry_is_enabled
    {
        get => entry_is_enabled;
        set
        {
            if (entry_is_enabled != value)
            {
                entry_is_enabled = value;
                OnPropertyChanged();
            }
        }
    }
    private bool entry_is_enabled = true;

    public MainPage()
    {
        InitializeComponent();
    }

    private async void ContentPage_Loaded(object sender, EventArgs e)
    {
        try
        {
            await ParseMADI.GetGroups();
            await ParseMADI.GetWeek();
        }
        catch
        {
            await DisplayAlert("Ошибка", "Не удалось загрузить расписание :(\nВозможно, нет соединения с интернетом.", "Ок");
            return;
        }

        if (!Preferences.Default.ContainsKey("id_group"))
            return;
        IdMADI.Id = IdMADI.LoadSavedID();
        EntryText = IdMADI.Id.Value;
    }

    private void DatePicker_DateSelected(object sender, DateChangedEventArgs e)
    {

    }

    private async void Entry_TextChanged(object sender, TextChangedEventArgs e)
    {
        if (ParseMADI.id_groups.Any(x => x.Value.ToLower().Equals(e.NewTextValue.ToLower())))
        {
            Entry_is_enabled = false;
            Entry_is_enabled = true;

            IdMADI.Id = ParseMADI.id_groups.Where(x => x.Value.ToLower().Equals(e.NewTextValue.ToLower())).Single();
            try
            {
                shedule = await ParseMADI.GetShedule(IdMADI.Id.Key);
            }
            catch
            {
                await DisplayAlert("Ошибка", "Не удалось загрузить расписание :(\nВозможно, нет соединения с интернетом.", "Ок");
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

            for (int i = 0; i < 7; i++)
                foreach (var lesson in shedule[i].Lessons)
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
            Days.Clear();
            Days = new ObservableCollection<Day>(days1.Concat(days2));
            shedule.Clear();

            daysCarousel.ScrollTo(8);//сделать на двухсторонней привязке данных, мейби заработает. Не скроллит, оставляет первый элемент
        }
    }
}