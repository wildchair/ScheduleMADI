namespace ScheduleCore.Models.DTO
{
    public class ProfessorDTO
    {
        public required int Id { get; set; }
        public required string Name { get; set; }
        public required List<LessonForProfessorDTO> Lessons { get; set; }
        //public required List<Examination> Exams { get; set; }
    }
}