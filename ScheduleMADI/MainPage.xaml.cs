using System.Collections.ObjectModel;

namespace ScheduleMADI;

public partial class MainPage : ContentPage
{
    public ObservableCollection<SubjectCard> Cards { get => cards; set => cards = value; }

    private ObservableCollection<SubjectCard> cards = new();
    public MainPage()
    {
        InitializeComponent();
        lst.ItemsSource = Cards;
    }

    private async void ContentPage_Loaded(object sender, EventArgs e)
    {
        await ParseMADI.GetWeek();
        await ParseMADI.GetGroups();
        await ParseMADI.GetShedule("8716");

        var day = ParseMADI.days.Single(x => x.Name == DateTime.Now.DayOfWeek);
        foreach (var lesson in day.Lessons)
        {
            Cards.Add(new SubjectCard { CardDay = lesson.Day, CardName = lesson.Name, CardProf = lesson.Prof, CardRoom = lesson.Room, CardTime = lesson.Time, CardType = lesson.Type });
        }
        return;
    }

    private void DatePicker_DateSelected(object sender, DateChangedEventArgs e)
    {
        var day = ParseMADI.days.Single(x => x.Name == e.NewDate.DayOfWeek);
        Cards.Clear();
        foreach (var lesson in day.Lessons)
            Cards.Add(new SubjectCard { CardDay = lesson.Day, CardName = lesson.Name, CardProf = lesson.Prof, CardRoom = lesson.Room, CardTime = lesson.Time, CardType = lesson.Type });
    }
}

