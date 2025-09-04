using ScheduleMADI.Interfaces;
using System.Collections.ObjectModel;

namespace ScheduleMADI.ViewModel
{
    public class TestScheduleTargetProvider : IScheduleTargetProvider
    {
        public ReadOnlyCollection<ScheduleTarget> ScheduleTargets { get; set; } = new(
        [
            new() { Id = 1, Value = "Иван" },
            new() { Id = 2, Value = "2бАСУ" },
            new() { Id = 3, Value = "1бАСУ" }
        ]);
        public ScheduleTarget CurrentTarget 
        { 
            get => field;
            set
            {
                field = value;
                OnCurrentTargetChanged?.Invoke(this, new(value));
            }
        }
        public event EventHandler<ScheduleTargetChangedEventArgs> OnCurrentTargetChanged;
    }

    public class ScheduleTargetChangedEventArgs : EventArgs
    {
        public ScheduleTarget NewTarget;
        public ScheduleTargetChangedEventArgs(ScheduleTarget newTarget)
        {
            NewTarget = newTarget;
        }
    }
}
