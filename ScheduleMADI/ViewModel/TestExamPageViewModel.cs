using CommunityToolkit.Mvvm.ComponentModel;
using ScheduleCore.Models.RawModels;
using System.Collections.ObjectModel;

namespace ScheduleMADI.Interfaces;

public partial class TestExamPageViewModel : ObservableObject, IExamPageViewModel
{
    [ObservableProperty]
    public partial string ScheduleTarget { get; set; }
    [ObservableProperty]
    public partial ObservableCollection<Exam> Exams { get; set; } = new();
    [ObservableProperty]
    public partial string Placeholder { get; set; } = "Нет ниче";
    public bool IsLoading { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

    private readonly IScheduleTargetProvider _scheduleTargetProvider;

    public TestExamPageViewModel(IScheduleTargetProvider scheduleTargetProvider)
    {
        _scheduleTargetProvider = scheduleTargetProvider;
        _scheduleTargetProvider.OnCurrentTargetChanged += (sender, args) => ScheduleTarget = args.NewTarget.Value;
    }

    partial void OnScheduleTargetChanged(string value)
    {
        Exams.Clear();
        for (int i = 0; i < Random.Shared.Next(1, 7); i++)
        {
            Exams.Add(new() { CardName = "Test" });
        }
    }
}