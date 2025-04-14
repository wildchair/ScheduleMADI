namespace ScheduleCore.Models.DTO
{
    public class LessonDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public TypeOfWeek Week { get; set; }
        public TimeOnly Start { get; set; }
        public TimeOnly End { get; set; }
        public string? Type { get; set; }
        public List<GroupDto>? Groups { get; set; }
        public List<ProfessorDto>? Professors { get; set; }
        public DayOfWeek Day { get; set; }
        public string? Classroom { get; set; }

        public LessonDto(Lesson lesson)
        {
            Id = lesson.Id;
            Name = lesson.Name;
            Week = lesson.Week;
            Start = lesson.Start;
            End = lesson.End;
            Type = lesson.Type;
            Groups = new List<GroupDto>();
            Professors = new List<ProfessorDto>();
            Day = lesson.Day;
            Classroom = lesson.Classroom;


            foreach (var group in lesson.Groups)
                Groups.Add(new(group));
            foreach(var professor in lesson.Professors)
                Professors.Add(new(professor));
        }
    }
}