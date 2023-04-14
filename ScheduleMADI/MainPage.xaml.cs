using System.Collections.ObjectModel;

namespace ScheduleMADI;

public partial class MainPage : ContentPage
{
    public ObservableCollection<SubjectCard> Cards { get => cards; set => cards = value; }
    private ObservableCollection<SubjectCard> cards = new();

    public DateTime MinDate { get => minDate; set => minDate = value; }
    private DateTime minDate = DateTime.Now.AddMonths(-1);
    public DateTime MaxDate { get => maxDate; set => maxDate = value; }
    private DateTime maxDate = DateTime.Now.AddMonths(2);

    public MainPage()
    {
        InitializeComponent();
    }

    private async void ContentPage_Loaded(object sender, EventArgs e)
    {
        await ParseMADI.GetWeek();
        await ParseMADI.GetGroups();
        await ParseMADI.GetShedule($"{ParseMADI.ID.Key}");

        var day = ParseMADI.days.Single(x => x.Name == DateTime.Now.DayOfWeek);
        foreach (var lesson in day.Lessons)
            if (lesson.Day == ParseMADI.Week || lesson.Day == "Еженедельно" 
                || (ParseMADI.Week == "Знаменатель" && lesson.Day=="Знам. 1 раз в месяц") 
                || (ParseMADI.Week == "Числитель" && lesson.Day == "Числ. 1 раз в месяц"))
                Cards.Add(new SubjectCard { CardDay = lesson.Day, CardName = lesson.Name, CardProf = lesson.Prof, CardRoom = lesson.Room, CardTime = lesson.Time, CardType = lesson.Type });

        return;
    }

    private void DatePicker_DateSelected(object sender, DateChangedEventArgs e)
    {
        var day = ParseMADI.days.Single(x => x.Name == e.NewDate.DayOfWeek);
        Cards.Clear();
        foreach (var lesson in day.Lessons)
            if (lesson.Day == ParseMADI.Week || lesson.Day == "Еженедельно"
                || (ParseMADI.Week == "Знаменатель" && lesson.Day == "Знам. 1 раз в месяц")
                || (ParseMADI.Week == "Числитель" && lesson.Day == "Числ. 1 раз в месяц"))
                Cards.Add(new SubjectCard { CardDay = lesson.Day, CardName = lesson.Name, CardProf = lesson.Prof, CardRoom = lesson.Room, CardTime = lesson.Time, CardType = lesson.Type });
    }

    private void This_Appearing(object sender, EventArgs e)
    {
        if(ParseMADI.reloaded)
        {
            var day = ParseMADI.days.Single(x => x.Name == DateTime.Now.DayOfWeek);
            Cards.Clear();
            foreach (var lesson in day.Lessons)
                if (lesson.Day == ParseMADI.Week || lesson.Day == "Еженедельно"
                    || (ParseMADI.Week == "Знаменатель" && lesson.Day == "Знам. 1 раз в месяц")
                    || (ParseMADI.Week == "Числитель" && lesson.Day == "Числ. 1 раз в месяц"))
                    Cards.Add(new SubjectCard { CardDay = lesson.Day, CardName = lesson.Name, CardProf = lesson.Prof, CardRoom = lesson.Room, CardTime = lesson.Time, CardType = lesson.Type });
            ParseMADI.reloaded = false;
            datePicker.Date = DateTime.Now.Date;
        }
    }
}