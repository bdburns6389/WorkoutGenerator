using System.Collections.Generic;
using WorkoutGenerator.Models;

namespace WorkoutGenerator.ViewModels
{
    public class ViewWorkoutViewModel
    {
        public IList<ExerciseWorkout> Exercises { get; set; }
        public Workout Workout { get; set; }
    }
}
