using System.Collections.ObjectModel;
using System.Windows.Input;

namespace ScheduleMADI.Interfaces;

public interface ISettingsPageViewModel
{
    string SearchText { get; set; }
    bool IsSearchEnabled { get; set; }
    ObservableCollection<ScheduleTarget> SearchResults { get; }
    bool IsResultsVisible { get; set; }
    ICommand SelectItemCommand { get; }
    string SupportCardText { get; }
    ICommand SupportCardTappedCommand { get; }
    string AppVersion { get; }
}

public class ScheduleTarget
{
    public int Id { get; set; }
    public string Value { get; set; } = string.Empty;
}
