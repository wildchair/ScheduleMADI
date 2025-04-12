using ScheduleCore.Models.Madi;

namespace ScheduleApi.Services.Interfaces
{
    public interface IGroupsService
    {
        public Task<MadiEntityRegistry> GetGroupsAsync();
    }
}
