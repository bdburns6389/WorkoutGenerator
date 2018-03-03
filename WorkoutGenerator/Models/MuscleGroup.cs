using System.Collections.Generic;

namespace RandomWorkout.Models
{
    public class MuscleGroup
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public IList<Exercise> Exercises { get; set; }
    }
}
