using Microsoft.EntityFrameworkCore;
using ScheduleCore.Models;

namespace ScheduleApi.Repository
{
    public class InMemoryDbContext : DbContext
    {
        public DbSet<Schedule> Schedules { get; set; }
        public DbSet<LessonNew> Lessons { get; set; }
        public InMemoryDbContext(DbContextOptions<InMemoryDbContext> options) : base(options) { }
    }
}
