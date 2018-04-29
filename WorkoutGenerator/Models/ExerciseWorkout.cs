using System;

namespace WorkoutGenerator.Models
{
    public class ExerciseWorkout
    {
        public Guid WorkoutID { get; set; }
        public Workout Workout { get; set; }

        public Guid ExerciseID { get; set; }
        public Exercise Exercise { get; set; }
    }
}
