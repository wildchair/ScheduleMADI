using ScheduleCore.Models.Old;

namespace ScheduleCore.Models
{
    public class LessonNew
    {
        public int Id { get; set; }
        public string? Classroom { get; set; }
        public string? Type { get; set; }
        public string? Time { get; set; }
        public required string Name { get; set; }
        public required TypeOfWeek Week { get; set; }

        public DayNew Day { get; set; }

        //public string? CardDay { get; set; }
        //public string? CardProf { get; set; }
    }
}
