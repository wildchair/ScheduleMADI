using Microsoft.EntityFrameworkCore;
using ScheduleCore.Models;

namespace ScheduleApi.Repository
{
    public class InMemoryDbContext : DbContext
    {
        public DbSet<Schedule> Schedules { get; set; }

        public InMemoryDbContext(DbContextOptions<InMemoryDbContext> options) : base(options) { }
        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    optionsBuilder.UseInMemoryDatabase("ScheduleDatabase");
        //}
    }
}
