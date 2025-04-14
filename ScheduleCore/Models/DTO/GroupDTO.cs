namespace ScheduleCore.Models.DTO
{
    public class GroupDto
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public GroupDto(Group group)
        {
            Id = group.Id;
            Name = group.Name;
        }
    }
}