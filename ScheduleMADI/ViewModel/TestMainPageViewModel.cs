using CommunityToolkit.Mvvm.ComponentModel;
using ScheduleCore.Models.Madi;
using ScheduleMADI.Interfaces;
using System.Collections.ObjectModel;

namespace ScheduleMADI.Test
{
    public partial class TestMainPageViewModel : ObservableObject, IMainPageViewModel
    {
        [ObservableProperty]
        public partial string ScheduleTarget { get; set; }
        [ObservableProperty]
        public partial DateTime MinimalDate { get; set; } = DateTime.Now.AddDays(-30);
        [ObservableProperty]
        public partial DateTime MaximalDate { get; set; } = DateTime.Now.AddDays(30);
        [ObservableProperty]
        public partial DateTime SelectedDate { get; set; } = DateTime.Now;
        [ObservableProperty]
        public partial bool IsEnabled_DataPicker { get; set; } = true;
        [ObservableProperty]
        public partial ObservableCollection<Day> Schedule { get; set; } =
        [
            new(DayOfWeek.Monday, [new(),new(),new(),new(),new()], "Ужуну"),
            new(DayOfWeek.Thursday, [new(),new(),new(),new(),new()], "Ужуну"),
            new(DayOfWeek.Wednesday, [new(),new(),new(),new(),new()], "Ужуну"),
            new(DayOfWeek.Saturday, [new(),new(),new(),new(),new()], "Ужуну"),
        ];
        [ObservableProperty]
        public partial string Placeholder { get; set; } = "Ниче нету";

        private IScheduleTargetProvider _scheduleTargetProvider;

        public TestMainPageViewModel(IScheduleTargetProvider scheduleTargetProvider)
        {
            _scheduleTargetProvider = scheduleTargetProvider;
            _scheduleTargetProvider.OnCurrentTargetChanged += (sender, args) => ScheduleTarget = args.NewTarget.Value;
        }
    }
}