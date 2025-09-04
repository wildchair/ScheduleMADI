using ScheduleCore.Models.RawModels;
using System.Collections.ObjectModel;

namespace ScheduleMADI.Interfaces;

public interface IExamPageViewModel
{
    bool IsLoading { get; set; }
    string ScheduleTarget { get; set; }
    ObservableCollection<Exam> Exams { get; set; }
    string Placeholder { get; set; }
}