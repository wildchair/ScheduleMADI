using System.ComponentModel.DataAnnotations;

namespace ScheduleCore.Models.RawModels
{
    public class Exam
    {
        [Key]
        public int Id { get; set; }
        public string? Time { get; set; }
        public string? Subject { get; set; }
        public string? Visitors { get; set; }
        public string? Classroom { get; set; }
    }
}
