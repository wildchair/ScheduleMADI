namespace ScheduleCore.Models
{
    public class Group
    {
        public required int Id { get; set; }
        public required string Name { get; set; }
        public required List<Lesson> Lessons { get; set; }
        //public required List<Examination> Exams { get; set; }
    }
}