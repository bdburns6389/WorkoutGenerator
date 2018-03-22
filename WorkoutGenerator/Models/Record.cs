using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WorkoutGenerator.Models
{
    public class Record
    {
        [Key]
        public int ID { get; set; }
        public string Sets { get; set; }
        public string Reps { get; set; }
        public string Weight { get; set; }
        public DateTime DateCreated { get; set; }

        //ForeignKey for User 
        [ForeignKey("Id")]
        public string OwnerId { get; set; }
        public virtual ApplicationUser User { get; set; }
        //ForeignKey for Exercises
        [ForeignKey("ID")]
        public int ExerciseID { get; set;}
        public Exercise Exercise { get; set; }
        //ForeignKey for Workouts
        [ForeignKey("ID")]
        public int WorkoutID { get; set; }
        public Workout Workout { get; set; }

    }
}
