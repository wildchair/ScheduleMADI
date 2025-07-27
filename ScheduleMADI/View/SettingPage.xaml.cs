using ScheduleMADI.Interfaces;

namespace ScheduleMADI;

public partial class SettingPage : ContentPage
{
    public SettingPage(ISettingsPageViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}