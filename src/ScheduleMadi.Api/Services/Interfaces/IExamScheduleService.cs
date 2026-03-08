using ScheduleMadi.Core.Models.Madi;

namespace ScheduleMadi.Api.Services.Interfaces
{
    public interface IExamScheduleService
    {
        public Task<IEnumerable<Exam>> GetExamScheduleAsync(int id);
    }
}
