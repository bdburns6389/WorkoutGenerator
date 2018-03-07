using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using RandomWorkout.Models;

namespace WorkoutGenerator.Models
{
    // Add profile data for application users by adding properties to the ApplicationUser class
    public class ApplicationUser : IdentityUser
    {
        //Should link to Exercise model to create One to Many relationship
        public IList<Exercise> Exercises { get; set; }
    }
}
