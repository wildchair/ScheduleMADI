using ScheduleCore.Models.Old;

namespace ScheduleCore.Models
{
    public class Schedule
    {
        public required int Id { get; set; }

        public required string Owner { get; set; }
        public required IEnumerable<DayNew> Days { get; set; }
    }
}