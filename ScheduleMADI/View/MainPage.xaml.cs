using System.ComponentModel;

namespace ScheduleMADI;

/// <summary>
/// ПУБЛИКОВАТЬ НУЖНО ВСЁ ИЗ "ОПУБЛИКОВАТЬ", НЕ ИЗ КОНСОЛИ!!!!!! КОНСОЛЬ КРИВО ПОДПИСЫВАЕТ!!!
/// </summary>
public partial class MainPage : ContentPage
{
    private readonly MainPageVM mainPageVM = new();
    private bool carouselLoaded;
    private bool restoringPosition;
    private bool pageAppeared;
    private bool acceptingUserPosition;
    private int restoreGeneration;

    public MainPage()
    {
        InitializeComponent();
        mainPageVM.PropertyChanged += MainPageVM_PropertyChanged;
        mainPageVM.Carousel.PropertyChanged += Carousel_PropertyChanged;
        ScheduleCarousel.HandlerChanging += (_, _) => InvalidateRestore();
        ScheduleCarousel.HandlerChanged += (_, _) => RestoreCarouselPosition();
        BindingContext = mainPageVM;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        pageAppeared = true;
        RestoreCarouselPosition();
    }

    protected override void OnDisappearing()
    {
        pageAppeared = false;
        InvalidateRestore();
        base.OnDisappearing();
    }

    private void InvalidateRestore()
    {
        restoringPosition = true;
        restoreGeneration++;
    }

    private async void Mainpage_Appearing(object sender, EventArgs e)
    {
        if (!Preferences.Default.ContainsKey("swipe_instr"))
        {
            await DisplayAlertAsync("Подсказка",
                "Перелистывайте расписание свайпом влево или вправо. " +
                "Конкретную дату можно выбрать в календаре.", "Ок");
            Preferences.Default.Set("swipe_instr", 1);
        }
    }

    private void Carousel_Loaded(object sender, EventArgs e)
    {
        carouselLoaded = true;
        RestoreCarouselPosition();
    }

    private void Carousel_Unloaded(object sender, EventArgs e)
    {
        carouselLoaded = false;
        InvalidateRestore();
    }

    private void Carousel_PositionChanged(object sender, PositionChangedEventArgs e)
    {
        if (!pageAppeared || !carouselLoaded || restoringPosition || !mainPageVM.Datepicker_is_enabled)
            return;

        acceptingUserPosition = true;
        try { mainPageVM.Carousel.Position = e.CurrentPosition; }
        finally { acceptingUserPosition = false; }
    }

    private void Carousel_PropertyChanged(object sender, PropertyChangedEventArgs e)
    {
        // Выбор даты в нативном календаре не обязан повторно вызывать OnAppearing страницы.
        if (!acceptingUserPosition && e.PropertyName == nameof(CarouselVM.Position))
            RestoreCarouselPosition();
    }

    private void MainPageVM_PropertyChanged(object sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(MainPageVM.Datepicker_is_enabled) && mainPageVM.Datepicker_is_enabled)
            RestoreCarouselPosition();
    }

    private void RestoreCarouselPosition()
    {
        restoringPosition = true;
        var generation = ++restoreGeneration;
        var position = mainPageVM.Carousel.Position;
        Dispatcher.Dispatch(() =>
        {
            if (generation != restoreGeneration || !pageAppeared || !carouselLoaded || !mainPageVM.Datepicker_is_enabled)
                return;

            try
            {
                // Position может уже совпадать с VM, пока нативная страница осталась на прежнем дне.
                ScheduleCarousel.Position = position;
                ScheduleCarousel.ScrollTo(position, position: ScrollToPosition.Center, animate: false);
            }
            finally
            {
                if (generation == restoreGeneration)
                    restoringPosition = false;
            }
        });
    }
}
