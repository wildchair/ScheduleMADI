namespace ScheduleCore.Models.Old
{
    public class DayNew
    {
        public int Id { get; set; }
        public DayOfWeek DayOfWeek { get; set; }
        public IEnumerable<LessonNew> Lessons { get; set; }
        public Schedule Schedule { get; set; }
    }
}
