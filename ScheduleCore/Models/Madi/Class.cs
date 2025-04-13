using Microsoft.EntityFrameworkCore;

namespace ScheduleCore.Models.Madi
{
#warning Need renaming
    [Owned]
    public class Class
    {
        public string? Time { get; set; }
        public string? Day { get; set; }
        public string? Type { get; set; }
        public string? Name { get; set; }
        public string? Visitors { get; set; }
        public string? Classroom { get; set; }
    }
}