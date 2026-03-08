using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;

namespace ScheduleMADI;

public partial class SettingPage : ContentPage
{
    public SettingPage()
    {
        InitializeComponent();
        BindingContext = new SettingPageVM();
    }

    private async void TapGestureRecognizer_Tapped(object sender, TappedEventArgs e)
    {
        CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        await Clipboard.Default.SetTextAsync("5536 9137 8567 4439");
        var toast = Toast.Make("Скопировано", ToastDuration.Short);
        await toast.Show(cancellationTokenSource.Token);
    }
}