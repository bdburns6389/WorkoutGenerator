using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WorkoutGenerator.Models;

namespace WorkoutGenerator.Models
{
    public class Exercise
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public MuscleGroup MuscleGroup { get; set; }
        public int MuscleGroupID { get; set; }
        public DateTime DateCreated { get; set; }
        [Key]
        public int ID { get; set; }

        //OwnerId and ApplicationUser User is used to make a OneToMany Relationship 
        [ForeignKey("Id")]
        public string  OwnerId { get; set; }
        public virtual ApplicationUser User { get; set; }

    

        public IList<ExerciseWorkout> ExerciseWorkouts { get; set; } = new List<ExerciseWorkout>();
        public IList<ExerciseRecord> ExerciseRecords { get; set; } = new List<ExerciseRecord>();
    }

}

