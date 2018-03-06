using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WorkoutGenerator.Models;

namespace RandomWorkout.Models
{
    public class Exercise
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public MuscleGroup MuscleGroup { get; set; }
        public int MuscleGroupID { get; set; }
        [Key]
        public int ID { get; set; }

        public string UserId { get; set; }
        [ForeignKey("ID")]
        public virtual ApplicationUser User { get; set; }

        public IList<ExerciseWorkout> ExerciseWorkouts { get; set; } = new List<ExerciseWorkout>();
    }

}

