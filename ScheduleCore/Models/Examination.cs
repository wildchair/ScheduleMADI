namespace ScheduleCore.Models
{
    public class Examination
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public required DateTime DateTime { get; set; }
        public required string Classroom { get; set; }
        public required List<Group> Groups { get; set; }
        public required List<Professor> Professors { get; set; }
    }
}