using ScheduleMadi.Core.Models.Madi;

namespace ScheduleMadi.Api.Services.Interfaces
{
    public interface IGroupsService
    {
        public Task<MadiEntityRegistry> GetGroupsAsync();
    }
}
