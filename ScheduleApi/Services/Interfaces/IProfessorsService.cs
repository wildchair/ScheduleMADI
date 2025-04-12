using ScheduleCore.Models.Madi;

namespace ScheduleApi.Services.Interfaces
{
    public interface IProfessorsService
    {
        public Task<MadiEntityRegistry> GetProfessorsAsync();
    }
}
