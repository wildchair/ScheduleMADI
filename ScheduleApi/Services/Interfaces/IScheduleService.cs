using ScheduleCore.Models.Madi;

namespace ScheduleApi.Services.Interfaces
{
    public interface IScheduleService
    {
        public Task<Schedule> GetScheduleAsync(int id);
    }
}