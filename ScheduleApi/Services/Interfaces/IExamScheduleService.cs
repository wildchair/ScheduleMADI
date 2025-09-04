using ScheduleCore.Models.RawModels;

namespace ScheduleApi.Services.Interfaces
{
    public interface IExamScheduleService
    {
        public Task<IEnumerable<Exam>> GetExamScheduleAsync(int id);
    }
}
