using ScheduleCore.Models.Old;

namespace ScheduleApi.Services.Interfaces
{
    public interface IProfessorsService
    {
        public Task<MadiEntityRegistry> GetProfessorsAsync();
    }
}
