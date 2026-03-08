using ScheduleMadi.Core.Models.Madi;

namespace ScheduleMadi.Api.Services.Interfaces
{
    public interface IScheduleService
    {
        public Task<Schedule> GetScheduleAsync(int id);
    }
}