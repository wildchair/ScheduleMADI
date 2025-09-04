using ScheduleMADI.Interfaces;

namespace ScheduleMADI;

public partial class MainPage : ContentPage
{
    public MainPage(IMainPageViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}