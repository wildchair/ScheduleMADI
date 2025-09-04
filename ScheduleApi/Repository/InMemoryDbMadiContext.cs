using Microsoft.EntityFrameworkCore;
using ScheduleCore.Models;
using ScheduleCore.Models.Madi;
using ScheduleCore.Models.RawModels;

namespace ScheduleApi.Repository
{
    public class InMemoryDbMadiContext : DbContext
    {
        public DbSet<Schedule> Schedules { get; set; }
        public DbSet<Day> Days { get; set; }
        public DbSet<Exam> Exams { get; set; }
        public DbSet<ScheduleCore.Models.RawModels.Lesson> Classes { get; set; }

        public InMemoryDbMadiContext(DbContextOptions options) : base(options)
        {
        }

    }
}
