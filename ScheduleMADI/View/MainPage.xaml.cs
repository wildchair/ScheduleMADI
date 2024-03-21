namespace ScheduleMADI;

/// <summary>
/// ПУБЛИКОВАТЬ НУЖНО ВСЁ ИЗ "ОПУБЛИКОВАТЬ", НЕ ИЗ КОНСОЛИ!!!!!! КОНСОЛЬ КРИВО ПОДПИСЫВАЕТ!!!
/// </summary>
public partial class MainPage : ContentPage
{
    MainPageVM mainPageVM = new();
    public MainPage()
    {
        InitializeComponent();
        BindingContext = mainPageVM;
    }

    private async void Mainpage_Appearing(object sender, EventArgs e)
    {
        if (!Preferences.Default.ContainsKey("tap_instr"))
        {
            await DisplayAlert("Подсказка", "Двойное касание правой/левой части экрана позволяет переключаться между днями." +
                "Конкретную дату можно выбрать в календаре.", "Ок");
            Preferences.Default.Set("tap_instr", 1);
        }
    }

    private void TapGestureRecognizer_Tapped(object sender, TappedEventArgs e)
    {
        Vibration.Vibrate(30);

        if (Window.Width / 2 > e.GetPosition(null).Value.X && mainPageVM.withoutCarouselVM.DatepickerDate > mainPageVM.MinDate)
            mainPageVM.withoutCarouselVM.DatepickerDate = mainPageVM.withoutCarouselVM.DatepickerDate.AddDays(-1);
        else if (Window.Width / 2 < e.GetPosition(null).Value.X && mainPageVM.withoutCarouselVM.DatepickerDate < mainPageVM.MaxDate)
            mainPageVM.withoutCarouselVM.DatepickerDate = mainPageVM.withoutCarouselVM.DatepickerDate.AddDays(1);
    }

    private void cv_ChildAdded(object sender, ElementEventArgs e)
    {
        if (DP.Date.Date == DateTime.Today.Date)
            Task.Run(() => (e.Element as SubjectCard).ProgressLoop());
        else if (DP.Date.Date < DateTime.Today.Date)
            Task.Run(() => (e.Element as SubjectCard).ProgressTo(1, 500, Easing.Linear));
    }
}