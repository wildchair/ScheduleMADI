using System.ComponentModel.DataAnnotations;

namespace ScheduleCore.Models
{
    public class Owner
    {
        [Key]
        public required int Id { get; set; }
        public required string Name { get; set; }
        public required IEnumerable<Lesson> Lessons { get; set; }
        public required IEnumerable<Exam> Exams { get; set; }
    }
}