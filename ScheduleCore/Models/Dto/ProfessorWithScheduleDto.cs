namespace ScheduleCore.Models.DTO
{
    public class ProfessorWithScheduleDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<LessonDto> Lessons { get; set; }

        public ProfessorWithScheduleDto(Group group)
        {
            Id = group.Id;
            Name = group.Name;

            Lessons = new();

            foreach (var lesson in group.Lessons)
                Lessons.Add(new LessonDto(lesson));
        }
    }
}