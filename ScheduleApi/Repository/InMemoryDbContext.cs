using Microsoft.EntityFrameworkCore;
using ScheduleCore.Models;

namespace ScheduleApi.Repository
{
    public class InMemoryDbContext : DbContext
    {
        public DbSet<Owner> Owners { get; set; }
        public DbSet<Lesson> Lessons { get; set; }
        public InMemoryDbContext(DbContextOptions<InMemoryDbContext> options) : base(options) { }
    }
}