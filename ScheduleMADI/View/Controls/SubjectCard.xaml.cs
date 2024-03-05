namespace ScheduleMADI;

public partial class SubjectCard : ContentView
{
    public static readonly BindableProperty CardTimeProperty = BindableProperty.Create(nameof(CardTime), typeof(string), typeof(SubjectCard), string.Empty);
    public string CardTime
    {
        get => (string)GetValue(SubjectCard.CardTimeProperty);
        set => SetValue(SubjectCard.CardTimeProperty, value);
    }

    public static readonly BindableProperty CardDayProperty = BindableProperty.Create(nameof(CardDay), typeof(string), typeof(SubjectCard), string.Empty);
    public string CardDay
    {
        get => (string)GetValue(SubjectCard.CardDayProperty);
        set => SetValue(SubjectCard.CardDayProperty, value);
    }

    public static readonly BindableProperty CardTypeProperty = BindableProperty.Create(nameof(CardType), typeof(string), typeof(SubjectCard), string.Empty);
    public string CardType
    {
        get => (string)GetValue(SubjectCard.CardTypeProperty);
        set => SetValue(SubjectCard.CardTypeProperty, value);
    }

    public static readonly BindableProperty CardNameProperty = BindableProperty.Create(nameof(CardName), typeof(string), typeof(SubjectCard), string.Empty);
    public string CardName
    {
        get => (string)GetValue(SubjectCard.CardNameProperty);
        set => SetValue(SubjectCard.CardNameProperty, value);
    }

    public static readonly BindableProperty CardProfProperty = BindableProperty.Create(nameof(CardProf), typeof(string), typeof(SubjectCard), string.Empty);
    public string CardProf
    {
        get => (string)GetValue(SubjectCard.CardProfProperty);
        set => SetValue(SubjectCard.CardProfProperty, value);
    }

    public static readonly BindableProperty CardRoomProperty = BindableProperty.Create(nameof(CardRoom), typeof(string), typeof(SubjectCard), string.Empty);
    public string CardRoom
    {
        get => (string)GetValue(SubjectCard.CardRoomProperty);
        set => SetValue(SubjectCard.CardRoomProperty, value);
    }

    public SubjectCard()
    {
        InitializeComponent();
    }

    public async Task ProgressLoop()
    {
        string[] times;
        TimeSpan start;
        TimeSpan end;

        if (CardTime == null)
        {
            start = TimeSpan.FromMinutes(0);
            end = TimeSpan.FromMinutes(1440);
        }
        else
        {
            times = CardTime.Split("-");
            start = TimeSpan.Parse(times[0]);
            end = TimeSpan.Parse(times[1]);
        }

        var duration = (double)(end - start).TotalMinutes;

        while (DateTime.Now.TimeOfDay < end)
        {
            if (DateTime.Now.TimeOfDay > start)
                await bar.ProgressTo((DateTime.Now.TimeOfDay - start).TotalMinutes / duration, 500, Easing.Linear);
            await Task.Delay(60000);
        }

        if (DateTime.Now.TimeOfDay >= end)
            await bar.ProgressTo(1, 500, Easing.Linear);
    }
    public async Task ProgressTo(double value, uint length, Easing easing)
    {
        await bar.ProgressTo(value, length, easing);
    }
}