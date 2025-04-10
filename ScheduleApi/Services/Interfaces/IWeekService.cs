using ScheduleCore.Models.Old;

namespace ScheduleApi.Services.Interfaces
{
    public interface IWeekService
    {
        public Task<TypeOfWeek> GetTypeOfWeekAsync();
    }
}
