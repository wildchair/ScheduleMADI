namespace ScheduleMADI;

public partial class MainPage : ContentPage
{
    public MainPage()
    {
        InitializeComponent();
    }

    private async void ContentPage_Loaded(object sender, EventArgs e)
    {
        await Groups.GetList();
        await Groups.GetShedule();
        return;
    }
}

