namespace ScheduleCore.Models
{
    public class Schedule
    {
        public required int Id { get; set; }

        public required string Owner { get; set; }
        public required Dictionary<DayOfWeek, IEnumerable<LessonNew>> Days { get; set; }
    }
}