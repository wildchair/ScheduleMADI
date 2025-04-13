namespace ScheduleCore.Models.Madi
{
    public class Schedule
    {
        public int Id { get; set; }
        public required string OwnerName { get; set; }

        public DateTime LastFetch { get; set; }

        public List<Day>? Days { get; set; }
        public List<Exam>? Exams { get; set; }
    }
}