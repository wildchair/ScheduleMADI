namespace ScheduleCore.Models.DTO
{
    public class GroupDTO
    {
        public required int Id { get; set; }
        public required string Name { get; set; }
        public required List<LessonForGroupDTO> Lessons { get; set; }
        //public required List<Examination> Exams { get; set; }
    }
}