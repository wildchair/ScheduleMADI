namespace ScheduleMADI;

public partial class ExamCard : ContentView
{
    public static readonly BindableProperty CardTimeProperty = BindableProperty.Create(nameof(CardTime), typeof(string), typeof(ExamCard), string.Empty);
    public string CardTime
    {
        get => (string)GetValue(ExamCard.CardTimeProperty);
        set => SetValue(ExamCard.CardTimeProperty, value);
    }

    public static readonly BindableProperty CardNameProperty = BindableProperty.Create(nameof(CardName), typeof(string), typeof(ExamCard), string.Empty);
    public string CardName
    {
        get => (string)GetValue(ExamCard.CardNameProperty);
        set => SetValue(ExamCard.CardNameProperty, value);
    }

    public static readonly BindableProperty CardProfProperty = BindableProperty.Create(nameof(CardProf), typeof(string), typeof(ExamCard), string.Empty);
    public string CardProf
    {
        get => (string)GetValue(ExamCard.CardProfProperty);
        set => SetValue(ExamCard.CardProfProperty, value);
    }

    public static readonly BindableProperty CardRoomProperty = BindableProperty.Create(nameof(CardRoom), typeof(string), typeof(ExamCard), string.Empty);
    public string CardRoom
    {
        get => (string)GetValue(ExamCard.CardRoomProperty);
        set => SetValue(ExamCard.CardRoomProperty, value);
    }

    public ExamCard()
	{
		InitializeComponent();
	}
}