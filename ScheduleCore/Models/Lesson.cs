using ScheduleCore.Models.Old;

namespace ScheduleCore.Models
{
    public class Lesson
    {
        public required string Name { get; set; }
        public required TypeOfWeek Week { get; set; }
        public TimeSpan Start { get; set; }
        public TimeSpan End { get; set; }
        public string? Type { get; set; }
        public string? Visitor { get; set; }
        public DayOfWeek Day { get; set; }
        public string? Classroom { get; set; }
    }
}
