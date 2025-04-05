using ScheduleCore.Models;

namespace ScheduleApi.Services.Interfaces
{
    public interface IGroupsService
    {
        public Task<MadiEntityRegistry> GetGroupsAsync();
    }
}
