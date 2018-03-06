using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WorkoutGenerator.Data.Migrations;

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

        public int UserId { get; set; }
        [ForeignKey("UserId")]
        public virtual ApplicationUser User { get; set; }

        public IList<ExerciseWorkout> ExerciseWorkouts { get; set; } = new List<ExerciseWorkout>();
    }

}

