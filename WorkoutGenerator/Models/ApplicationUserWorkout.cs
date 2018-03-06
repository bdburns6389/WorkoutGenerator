using RandomWorkout.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WorkoutGenerator.Models
{
    public class ApplicationUserWorkout
    {
        public int WorkoutID { get; set; }
        public Workout Workout { get; set; }

        public int UserID { get; set; }
        public ApplicationUser User { get; set; }
    }
}
