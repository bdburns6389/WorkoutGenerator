namespace WorkoutGenerator.Models
{
    public class ExerciseWorkout
    {
        public int WorkoutID { get; set; }
        public Workout Workout { get; set; }

        public int ExerciseID { get; set; }
        public Exercise Exercise { get; set; }
    }
}
