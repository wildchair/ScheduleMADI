using CommunityToolkit.Mvvm.ComponentModel;
using ScheduleCore.Models.Madi;
using System.Collections.ObjectModel;

namespace ScheduleMADI.Interfaces
{
    public interface IMainPageViewModel
    {
        DateTime MinimalDate { get; set; }
        DateTime MaximalDate { get; set; }
        DateTime SelectedDate { get; set; }
        string ScheduleTarget { get; set; }
        bool IsEnabled_DataPicker { get; set; }
        ObservableCollection<Day> Schedule { get; set; }
        string Placeholder { get; set; }
    }
}