using ScheduleCore.Models;

namespace ScheduleApi.Services.Interfaces
{
    public interface IProfessorsService
    {
        public Task<MadiEntityRegistry> GetProfessorsAsync();
    }
}
