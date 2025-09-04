using CommunityToolkit.Maui.Core.Extensions;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace ScheduleMADI.Interfaces;

public partial class TestSettingsPageViewModel : ObservableObject, ISettingsPageViewModel
{
    public string AppVersion { get; } = AppInfo.Current.VersionString;
    public string SupportCardText { get; } = "5536 9137 8567 4439";
    [ObservableProperty]
    public partial string SearchText { get; set; } = String.Empty;
    [ObservableProperty]
    public partial bool IsSearchEnabled { get; set; } = true;
    [ObservableProperty]
    public partial bool IsResultsVisible { get; set; } = true;
    [ObservableProperty]
    public partial ObservableCollection<ScheduleTarget> SearchResults { get; set; }
    ICommand ISettingsPageViewModel.SupportCardTappedCommand => SupportCardTappedCommand;
    ICommand ISettingsPageViewModel.SelectItemCommand => SelectItemCommand;

    private readonly IScheduleTargetProvider _provider;
    public TestSettingsPageViewModel(IScheduleTargetProvider provider)
    {
        _provider = provider;
        SearchResults = _provider.ScheduleTargets.ToObservableCollection();
    }

    [RelayCommand]
    private async Task SupportCardTapped()
    {
        await Clipboard.Default.SetTextAsync(SupportCardText);
        //var toast = Toast.Make("Скопировано", ToastDuration.Short);
        //await toast.Show(CancellationToken.None);
    }

    [RelayCommand]
    private void SelectItem(ScheduleTarget item)
    {
        _provider.CurrentTarget = item;
        SearchText = item.Value;
    }

    partial void OnSearchTextChanged(string value)
    {
        value = value.ToLower();
        SearchResults.Clear();

        IsResultsVisible = !_provider.ScheduleTargets.Any(x => x.Value.ToLower().Equals(value));

        var searched = _provider.ScheduleTargets.Where(x => x.Value.ToLower().Contains(value));
        foreach (var item in searched)
            SearchResults.Add(item);
    }
}