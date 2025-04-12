namespace ScheduleCore.Models.Madi
{
    public class Schedule
    {
        public int Id { get; set; }
        public string Owner { get; set; }

        public List<Day>? Days { get; set; }
        public List<Exam>? Exams { get; set; }
    }
}