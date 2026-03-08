using Microsoft.EntityFrameworkCore;
using ScheduleMadi.Core.Models;

namespace ScheduleMadi.Api.Repository
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