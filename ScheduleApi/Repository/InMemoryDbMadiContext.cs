using Microsoft.EntityFrameworkCore;
using ScheduleCore.Models;
using ScheduleCore.Models.Madi;

namespace ScheduleApi.Repository
{
    public class InMemoryDbMadiContext : DbContext
    {
        public DbSet<Schedule> Schedules { get; set; }

        public InMemoryDbMadiContext(DbContextOptions options) : base(options)
        {
        }

    }
}
