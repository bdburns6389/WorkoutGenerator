using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WorkoutGenerator.Models
{//TODO Need an Exercise ID
    public class Record
    {
        [Key]
        public int RecordID { get; set; }
        public string Sets { get; set; }
        public string Reps { get; set; }
        public string Weight { get; set; }
        public DateTime DateCreated { get; set; }

        //ForeignKey for User 
        [ForeignKey("Id")]
        public string OwnerId { get; set; }
        public virtual ApplicationUser User { get; set; }
  
        //ForeignKey for Workouts//FOREIGNKEY GOES ON WORKOUT WORKOUT?j
        public int WorkoutID { get; set; }
        [ForeignKey("WorkoutID")]
        public virtual Workout Workout { get; set; }

        //ForeignKey for Exercise
        public int FK_ExerciseID { get; set; }
        [ForeignKey("ExerciseID")]
        public virtual Exercise Exercise { get; set; }

        public IList<ExerciseRecord> ExerciseRecords { get; set; } = new List<ExerciseRecord>();
    }
}
