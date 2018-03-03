using System.Collections.Generic;

namespace RandomWorkout.Models
{
    public class Workout
    {
        public int ID { get; set; }
        public string Name { get; set; }

        public IList<ExerciseWorkout> ExerciseWorkouts { get; set; } = new List<ExerciseWorkout>();
    }
}
 
