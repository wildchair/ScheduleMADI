using ScheduleCore.Models;

namespace ScheduleApi.Services.Interfaces
{
    public interface IScheduleService
    {
        public Task<IEnumerable<Day>> GetScheduleAsync(int id);
    }
}
