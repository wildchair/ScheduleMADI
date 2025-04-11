namespace ScheduleCore.Models
{
    public class Owner
    {
        public required int Id { get; set; }
        public required string Name { get; set; }
        public required IEnumerable<Lesson> Lessons { get; set; }
    }
}
