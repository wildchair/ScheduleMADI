using ScheduleMadi.Core.Models;

namespace ScheduleMadi.Api.Services.Interfaces
{
    public interface IWeekService
    {
        public Task<TypeOfWeek> GetTypeOfWeekAsync();
    }
}
