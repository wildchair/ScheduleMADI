using System.ComponentModel.DataAnnotations;

namespace ScheduleCore.Models.RawModels
{
    public class Exam
    {
        [Key]
        public int Id { get; set; }
        public string? CardDateTime { get; set; }
        public string? CardName { get; set; }
        public string? CardProf { get; set; }
        public string? CardRoom { get; set; }
    }
}
