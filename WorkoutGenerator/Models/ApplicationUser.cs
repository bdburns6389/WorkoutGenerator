using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace WorkoutGenerator.Models
{
    // Add profile data for application users by adding properties to the ApplicationUser class
    public class ApplicationUser : IdentityUser
    {
        //Should link to Exercise model to create One to Many relationship
        public IList<Exercise> Exercises { get; set; }

        public IList<Workout> Workouts { get; set; }

        public IList<MuscleGroup> MuscleGroups { get; set; }

        public IList<Record> Records { get; set; }
    }
}
