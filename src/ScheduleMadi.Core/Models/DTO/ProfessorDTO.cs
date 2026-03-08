using ScheduleMadi.Core.Models;

namespace ScheduleMadi.Core.Models.DTO
{
    public class ProfessorDto
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public ProfessorDto(Professor professor)
        {
            Id = professor.Id;
            Name = professor.Name;
        }
    }
}