using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WorkoutGenerator.Models;

namespace WorkoutGenerator.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<Exercise> Exercises { get; set; }
        public DbSet<MuscleGroup> MuscleGroups { get; set; }

        public DbSet<Workout> Workouts { get; set; }
        public DbSet<Record> Records { get; set; }
        public DbSet<ExerciseWorkout> ExerciseWorkouts { get; set; }
        public DbSet<ExerciseRecord> ExerciseRecords { get; set; }
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);
            builder.Entity<ExerciseWorkout>()
                .HasKey(c => new { c.ExerciseID, c.WorkoutID });
            builder.Entity<ExerciseRecord>()
                .HasKey(c => new { c.ExerciseID, c.RecordID });
      
        }
    }
}
