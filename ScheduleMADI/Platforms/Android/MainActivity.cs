using Android.App;
using Android.Content.PM;
using Android.OS;

namespace ScheduleMADI;

[Activity(Theme = "@style/Maui.SplashTheme", LaunchMode = LaunchMode.SingleInstance, MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize | ConfigChanges.Density)]
public class MainActivity : MauiAppCompatActivity
{
}
