using ScheduleCore.Models.Old;

namespace ScheduleCore.Models
{
    public class Lesson
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public required TypeOfWeek Week { get; set; }
        public TimeSpan Start { get; set; }
        public TimeSpan End { get; set; }
        public string? Type { get; set; }
        public List<int> Groups { get; set; }
        public List<int> Professors { get; set; }
        public DayOfWeek Day { get; set; }
        public string? Classroom { get; set; }
    }
}
