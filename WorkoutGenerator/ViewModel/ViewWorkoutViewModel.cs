using RandomWorkout.Models;
using System.Collections.Generic;

namespace RandomWorkout.ViewModels
{
    public class ViewWorkoutViewModel
    {
        public IList<ExerciseWorkout> Exercises { get; set; }
        public Workout Workout { get; set; }
    }
}
