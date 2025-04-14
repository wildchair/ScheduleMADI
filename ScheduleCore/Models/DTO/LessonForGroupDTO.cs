namespace ScheduleCore.Models.DTO
{
    public class LessonForGroupDTO
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public required TypeOfWeek Week { get; set; }
        public TimeSpan Start { get; set; }
        public TimeSpan End { get; set; }
        public string? Type { get; set; }
        public required List<ProfessorForGroupDTO> Professors { get; set; }
        public DayOfWeek Day { get; set; }
        public string? Classroom { get; set; }
    }
}