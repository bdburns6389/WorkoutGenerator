using System.Collections.Generic;

namespace RandomWorkout.Models
{
    public class Exercise
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public MuscleGroup MuscleGroup { get; set; }
        public int MuscleGroupID { get; set; }
        public int ID { get; set; }

        public IList<ExerciseWorkout> ExerciseWorkouts { get; set; } = new List<ExerciseWorkout>();
    }

}

