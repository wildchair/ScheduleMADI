using Microsoft.EntityFrameworkCore;
using ScheduleMadi.Core.Models.Madi;

namespace ScheduleMadi.Api.Repository
{
    public class InMemoryDbMadiContext : DbContext
    {
        public DbSet<Schedule> Schedules { get; set; }

        public InMemoryDbMadiContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Schedule>(schedule =>
            {
                schedule.OwnsMany(s => s.Days, day =>
                {
                    day.WithOwner();
                    day.OwnsMany(d => d.Lessons, lesson =>
                    {
                        lesson.WithOwner();
                    });
                });

                schedule.OwnsMany(s => s.Exams, exam =>
                {
                    exam.WithOwner();
                });
            });
        }

    }
}
