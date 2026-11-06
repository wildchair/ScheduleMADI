using Microsoft.Maui.Dispatching;

namespace ScheduleMADI;

public partial class SubjectCard : ContentView
{
    public static readonly BindableProperty CardDateProperty = BindableProperty.Create(nameof(CardDate), typeof(DateTime), typeof(SubjectCard), default(DateTime), propertyChanged: OnProgressInputChanged);
    public DateTime CardDate
    {
        get => (DateTime)GetValue(CardDateProperty);
        set => SetValue(CardDateProperty, value);
    }

    public static readonly BindableProperty CardTimeProperty = BindableProperty.Create(nameof(CardTime), typeof(string), typeof(SubjectCard), string.Empty, propertyChanged: OnProgressInputChanged);
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

    private readonly IDispatcherTimer progressTimer;
    private bool isLoaded;

    public SubjectCard()
    {
        InitializeComponent();
        progressTimer = Dispatcher.CreateTimer();
        progressTimer.Interval = TimeSpan.FromSeconds(1);
        progressTimer.Tick += (_, _) => UpdateProgress();
        Loaded += (_, _) =>
        {
            isLoaded = true;
            UpdateProgress();
        };
        Unloaded += (_, _) =>
        {
            isLoaded = false;
            progressTimer.Stop();
        };
    }

    private static void OnProgressInputChanged(BindableObject bindable, object oldValue, object newValue) =>
        ((SubjectCard)bindable).UpdateProgress();

    private void UpdateProgress()
    {
        if (!isLoaded)
            return;

        var now = DateTime.Now;
        bar.Progress = LessonProgress.Calculate(CardDate, CardTime, now);
        if (CardDate.Date == now.Date && bar.Progress < 1)
        {
            if (!progressTimer.IsRunning)
                progressTimer.Start();
        }
        else
            progressTimer.Stop();
    }
}
