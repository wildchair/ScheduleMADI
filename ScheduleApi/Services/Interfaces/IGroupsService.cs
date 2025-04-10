using ScheduleCore.Models.Old;

namespace ScheduleApi.Services.Interfaces
{
    public interface IGroupsService
    {
        public Task<MadiEntityRegistry> GetGroupsAsync();
    }
}
