using Microsoft.EntityFrameworkCore;
using ScheduleCore.Models;

namespace ScheduleApi.Repository
{
    public class InMemoryDbContext : DbContext
    {
        public DbSet<Group> Groups { get; set; }
        public DbSet<Lesson> Lessons { get; set; }
        public DbSet<Examination> Exams { get; set; }
        public DbSet<Professor> Professors { get; set; }
        public InMemoryDbContext(DbContextOptions<InMemoryDbContext> options) : base(options) { }
    }
}