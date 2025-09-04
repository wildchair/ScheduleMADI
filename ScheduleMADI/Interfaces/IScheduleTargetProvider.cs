using ScheduleMADI.ViewModel;
using System.Collections.ObjectModel;

namespace ScheduleMADI.Interfaces
{
    public interface IScheduleTargetProvider
    {
        ReadOnlyCollection<ScheduleTarget> ScheduleTargets { get; set; }
        ScheduleTarget CurrentTarget { get; set; }
        event EventHandler<ScheduleTargetChangedEventArgs> OnCurrentTargetChanged;
    }
}
