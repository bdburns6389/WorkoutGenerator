using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace RandomWorkout.Models
{
    public class MuscleGroup
    {
        [Key]
        public int ID { get; set; }
        public string Name { get; set; }
        public IList<Exercise> Exercises { get; set; }
    }
}
