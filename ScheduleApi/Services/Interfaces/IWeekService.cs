using ScheduleCore.Models;

namespace ScheduleApi.Services.Interfaces
{
    public interface IWeekService
    {
        public Task<TypeOfWeek> GetTypeOfWeekAsync();
    }
}
