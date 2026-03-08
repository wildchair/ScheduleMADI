using ScheduleCore.Models;

namespace ScheduleMadi.Api.Services.Interfaces
{
    public interface IDataBaseService
    {
        public Task<int> InitDbAsync();
    }
}