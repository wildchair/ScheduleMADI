using System.Collections.ObjectModel;

namespace ScheduleMADI;
[XamlCompilation(XamlCompilationOptions.Compile)]
public partial class MainPage : ContentPage
{
    private List<Day> schedule;//фулл расписание группы
    public Day CurrentDay//отображаемый день
    {
        get => currentDay;
        set
        {
            if (currentDay != value)
            {
                currentDay = value;
                OnPropertyChanged();
            }
        }
    }
    private Day currentDay;
    public DateTime MinDate { get => minDate; }//минимальная дата для датапикера
    private readonly DateTime minDate = DateTime.Now.AddMonths(-1);
    public DateTime MaxDate { get => maxDate; }//максимальна дата для датапикера
    private readonly DateTime maxDate = DateTime.Now.AddMonths(2);

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
    private string emptyString = "Загрузка расписания...";
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
    private string oldID;

    private DateTime today = DateTime.Now.Date;

    public DateTime DatepickerDate
    {
        get => datepickerDate;
        set
        {
            if (datepickerDate != value)
            {
                datepickerDate = value;
                OnPropertyChanged();
            }
        }
    }//текущая дата в датапикере
    private DateTime datepickerDate = DateTime.Now.Date;
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
    public MainPage()
    {
        InitializeComponent();
    }

    private async void Mainpage_Appearing(object sender, EventArgs e)
    {
        TapRecognizer.NumberOfTapsRequired = 0;
        if (ParseMADI.id_groups.Count == 0 || WeekMADI.Week == null)
        {
            EmptyString = "Загрузка расписания...";
            try
            {
                await ParseMADI.GetGroups();
                await ParseMADI.GetWeek();
            }
            catch
            {
                EmptyString = "Проверьте подключение к интернету и перезагрузите приложение.";
                Datepicker_is_enabled = false;
                return;
            }
        }

        if (IdMADI.Id.Value != null && IdMADI.Id.Value == oldID)
            return;

        if (IdMADI.Id.Value != null) ;//////////////
        else if (!Preferences.Default.ContainsKey("id_group"))
        {
            EmptyString = "Введите группу. \"Настройки\" -> \"Группа\"";
            return;
        }    
        else IdMADI.Id = IdMADI.LoadSavedID();

        GroupLabel = IdMADI.Id.Value;
        oldID = IdMADI.Id.Value;

        EmptyString = "Загрузка расписания...";
        try
        {
            schedule = await ParseMADI.GetShedule(IdMADI.Id.Key);
        }
        catch
        {
            EmptyString = "Проверьте подключение к интернету и перезагрузите приложение.";
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
            foreach (var lesson in schedule[i].Lessons)
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

        schedule.Clear();
        schedule = days1.Concat(days2).ToList();
        if (DatepickerDate.Date != today.Date)
            DatepickerDate = today.Date;
        else
            CurrentDay = schedule.Find(x => x.Name == today.DayOfWeek && x.TypeOfWeek == WeekMADI.Week);
        Datepicker_is_enabled = true;
        TapRecognizer.NumberOfTapsRequired = 2;

        if (!Preferences.Default.ContainsKey("tap_instr"))
        {
            await DisplayAlert("Подсказка", "Двойное касание правой/левой части экрана позволяет переключаться между днями." +
                "Конкретную дату можно выбрать в календаре.", "Ок");
            Preferences.Default.Set("tap_instr", 1);
        }
    }

    private void DatePicker_DateSelected(object sender, DateChangedEventArgs e)
    {
        CurrentDay = schedule.Find(x => x.Name == e.NewDate.DayOfWeek && x.TypeOfWeek == WeekMADI.WeekByDate(today, e.NewDate));
    }

    private void TapGestureRecognizer_Tapped(object sender, TappedEventArgs e)
    {
        Vibration.Vibrate(100);
        if (Window.Width / 2 > e.GetPosition(null).Value.X && DatepickerDate > MinDate)
            DatepickerDate = DatepickerDate.AddDays(-1);
        else if (Window.Width / 2 < e.GetPosition(null).Value.X && DatepickerDate < MaxDate)
            DatepickerDate = DatepickerDate.AddDays(1);
    }
}