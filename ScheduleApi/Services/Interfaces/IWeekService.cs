using ScheduleCore.Models.Madi;

namespace ScheduleApi.Services.Interfaces
{
    public interface IWeekService
    {
        public Task<TypeOfWeek> GetTypeOfWeekAsync();
    }
}
