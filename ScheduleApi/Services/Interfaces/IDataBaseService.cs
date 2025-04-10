using ScheduleCore.Models;

namespace ScheduleApi.Services.Interfaces
{
    public interface IDataBaseService
    {
        public Task<int> InitDbAsync();
    }
}