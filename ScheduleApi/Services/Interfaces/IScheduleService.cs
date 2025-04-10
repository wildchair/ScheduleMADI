using ScheduleCore.Models.Old;

namespace ScheduleApi.Services.Interfaces
{
    public interface IScheduleService
    {
        public Task<IEnumerable<Day>> GetScheduleAsync(int id);
    }
}
