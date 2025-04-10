namespace ScheduleCore.Models.Old
{
    public class DayNew
    {
        public DayOfWeek DayOfWeek { get; set; }
        public IEnumerable<LessonNew> Lessons { get; set; }
    }
}
