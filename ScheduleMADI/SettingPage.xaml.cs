using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
using System;
using static System.Net.Mime.MediaTypeNames;

namespace ScheduleMADI;

public partial class SettingPage : ContentPage
{
    public string VersionApp
    {
        get => versionApp;
        set
        {
            versionApp = value; OnPropertyChanged();
        }
    }
    private string versionApp = AppInfo.Current.VersionString;
    public string ButtonText
    {
        get => buttonText;
        set
        {
            if (buttonText != value)
            {
                buttonText = value;
                OnPropertyChanged();
            }
        }
    }
    private string buttonText = "Сохранить";
    public string EntryText
    {
        get => entryText;
        set
        {
            if (entryText != value)
            {
                entryText = value;
                OnPropertyChanged();
            }
        }
    }
    private string entryText;
    public bool Entry_is_enabled
    {
        get => entry_is_enabled;
        set
        {
            if (entry_is_enabled != value)
            {
                entry_is_enabled = value;
                OnPropertyChanged();
            }
        }
    }
    private bool entry_is_enabled = true;
    public bool Button_is_enabled
    {
        get => button_is_enabled;
        set
        {
            if (button_is_enabled != value)
            {
                button_is_enabled = value;
                OnPropertyChanged();
            }
        }
    }
    private bool button_is_enabled = false;

    public SettingPage()
    {
        InitializeComponent();
    }

    private void Entry_TextChanged(object sender, TextChangedEventArgs e)
    {
        if (IdMADI.Id.Value!=null && e.NewTextValue.ToLower() == IdMADI.Id.Value.ToLower())
        {
            ButtonText = "Сохранено";
            Button_is_enabled = false;
            return;
        }
        ButtonText = "Сохранить";
        if (ParseMADI.id_groups.Any(x => x.Value.ToLower().Equals(e.NewTextValue.ToLower())))
            Button_is_enabled = true;
        else
            Button_is_enabled = false;
    }

    private void Button_Clicked(object sender, EventArgs e)
    {
        IdMADI.Id = ParseMADI.id_groups.Where(x => x.Value.ToLower().Equals(EntryText.ToLower())).Single();
        ButtonText = "Сохранено";
        Button_is_enabled = false;

        Entry_is_enabled = false;
        Entry_is_enabled = true;

    }

    private void Settingpage_Appearing(object sender, EventArgs e)
    {
        if (IdMADI.Id.Value != null) ;//////////////
        else if (!Preferences.Default.ContainsKey("id_group"))
            return;
        else IdMADI.Id = IdMADI.LoadSavedID();

        EntryText = IdMADI.Id.Value;
    }

    private async void TapGestureRecognizer_Tapped(object sender, TappedEventArgs e)
    {
        CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        await Clipboard.Default.SetTextAsync("5536 9137 8567 4439");
        var toast = Toast.Make("Скопировано", ToastDuration.Short);
        await toast.Show(cancellationTokenSource.Token);
    }
}