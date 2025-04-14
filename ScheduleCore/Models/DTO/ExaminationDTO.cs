namespace ScheduleCore.Models.DTO
{
    public class ExaminationDTO
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public required DateTime DateTime { get; set; }
        public required string Classroom { get; set; }
        public required List<GroupDTO> Groups { get; set; }
        public required List<ProfessorDTO> Professors { get; set; }
    }
}