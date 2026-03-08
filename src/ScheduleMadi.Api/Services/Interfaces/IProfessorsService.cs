using ScheduleMadi.Core.Models.Madi;

namespace ScheduleMadi.Api.Services.Interfaces
{
    public interface IProfessorsService
    {
        public Task<MadiEntityRegistry> GetProfessorsAsync();
    }
}
