using Microsoft.EntityFrameworkCore;

namespace ScheduleCore.Models.Madi
{
    [Owned]
    public class Exam
    {
        public string? CardDateTime { get; set; }
        public string? CardName { get; set; }
        public string? CardProf { get; set; }
        public string? CardRoom { get; set; }
    }
}
