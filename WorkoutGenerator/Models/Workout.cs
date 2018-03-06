using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace RandomWorkout.Models
{
    public class Workout
    {
        [Key]
        public int ID { get; set; }
        public string Name { get; set; }

        public IList<ExerciseWorkout> ExerciseWorkouts { get; set; } = new List<ExerciseWorkout>();
    }
}
 
