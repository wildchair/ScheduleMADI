using System.ComponentModel.DataAnnotations;

namespace ScheduleCore.Models
{
    public class Exam
    {
        [Key]
        public required int Id { get; set; }
        public required string Subject { get; set; }
        public required List<Group> Groups { get; set; }
        public required List<Professor> Professors { get; set; }
        public string? Classroom { get; set; }
    }
}