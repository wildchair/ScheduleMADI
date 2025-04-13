namespace ScheduleCore.Models
{
    public class Professor
    {
        public required int Id { get; set; }
        public required string Name { get; set; }
        public List<Lesson> Lessons { get; set; } = [];
        //public required List<Examination> Exams { get; set; }
    }
}