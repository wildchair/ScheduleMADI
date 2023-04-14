using System.Collections.ObjectModel;
using System.Net.Sockets;

namespace ScheduleMADI;

public partial class MainPage : ContentPage
{
    public ObservableCollection<SubjectCard> Cards { get => cards; set => cards = value; }
    private ObservableCollection<SubjectCard> cards = new();

    public DateTime MinDate { get => minDate; set => minDate = value; }
    private DateTime minDate = DateTime.Now.AddMonths(-1);
    public DateTime MaxDate { get => maxDate; set => maxDate = value; }
    private DateTime maxDate = DateTime.Now.AddMonths(2);

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

        try
        {
            await ParseMADI.GetShedule($"{IdMADI.Id.Key}");
        }
        catch
        {
            await DisplayAlert("Ошибка", "Не удалось загрузить расписание :(\nВозможно, нет соединения с интернетом.", "Ок");
            return;
        }

        var day = ParseMADI.days.Single(x => x.Name == today.DayOfWeek);
        foreach (var lesson in day.Lessons)
            if (lesson.Day == WeekMADI.Week || lesson.Day == "Еженедельно" 
                || (WeekMADI.Week == "Знаменатель" && lesson.Day=="Знам. 1 раз в месяц") 
                || (WeekMADI.Week == "Числитель" && lesson.Day == "Числ. 1 раз в месяц"))
                Cards.Add(new SubjectCard { CardDay = lesson.Day, CardName = lesson.Name, CardProf = lesson.Prof, CardRoom = lesson.Room, CardTime = lesson.Time, CardType = lesson.Type });
        Datepicker_is_enabled = true;
        return;
    }

    private void DatePicker_DateSelected(object sender, DateChangedEventArgs e)
    {
        var day = ParseMADI.days.Single(x => x.Name == e.NewDate.DayOfWeek);
        var weekByDate = WeekMADI.WeekByDate(today, e.NewDate);
        Cards.Clear();
        foreach (var lesson in day.Lessons)
            if (lesson.Day == weekByDate || lesson.Day == "Еженедельно"
                || (weekByDate == "Знаменатель" && lesson.Day == "Знам. 1 раз в месяц")
                || (weekByDate == "Числитель" && lesson.Day == "Числ. 1 раз в месяц"))
                Cards.Add(new SubjectCard { CardDay = lesson.Day, CardName = lesson.Name, CardProf = lesson.Prof, CardRoom = lesson.Room, CardTime = lesson.Time, CardType = lesson.Type });
    }

    private void This_Appearing(object sender, EventArgs e)
    {
        if(ParseMADI.reloaded)
        {
            var day = ParseMADI.days.Single(x => x.Name == today.DayOfWeek);
            Cards.Clear();
            foreach (var lesson in day.Lessons)
                if (lesson.Day == WeekMADI.Week || lesson.Day == "Еженедельно"
                    || (WeekMADI.Week == "Знаменатель" && lesson.Day == "Знам. 1 раз в месяц")
                    || (WeekMADI.Week == "Числитель" && lesson.Day == "Числ. 1 раз в месяц"))
                    Cards.Add(new SubjectCard { CardDay = lesson.Day, CardName = lesson.Name, CardProf = lesson.Prof, CardRoom = lesson.Room, CardTime = lesson.Time, CardType = lesson.Type });
            ParseMADI.reloaded = false;
            Datepicker_is_enabled = true;
            datePicker.Date = today.Date;
        }
    }
}