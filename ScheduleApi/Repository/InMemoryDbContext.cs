using Microsoft.EntityFrameworkCore;
using ScheduleCore.Models;
using ScheduleCore.Models.Old;

namespace ScheduleApi.Repository
{
    public class InMemoryDbContext : DbContext
    {
        public DbSet<Schedule> Schedules { get; set; }
        public DbSet<DayNew> Days { get; set; }
        public InMemoryDbContext(DbContextOptions<InMemoryDbContext> options) : base(options) { }
        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    optionsBuilder.UseInMemoryDatabase("ScheduleDatabase");
        //}
    }
}
