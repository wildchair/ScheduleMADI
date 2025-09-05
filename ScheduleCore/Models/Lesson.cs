using System.ComponentModel.DataAnnotations;

namespace ScheduleCore.Models
{
    public class Lesson
    {
        [Key]
        public required int Id { get; set; }
        public required string Subject { get; set; }
        public required TypeOfWeek Period { get; set; }
        public TimeSpan Start { get; set; }
        public TimeSpan End { get; set; }
        public string? Type { get; set; }
        public required List<Group> Groups { get; set; }
        public required List<Professor> Professors { get; set; }
        public DayOfWeek Day { get; set; }
        public string? Classroom { get; set; }
    }
}