﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WorkoutGenerator.Models;

namespace WorkoutGenerator.Models
{
    public class Workout
    {
        [Key]
        public int WorkoutID { get; set; }
        public string Name { get; set; }
        public DateTime DateCreated { get; set; }

        //OwnerId and ApplicationUser User is used to make a OneToMany Relationship 
        [ForeignKey("Id")]
        public string OwnerId { get; set; }
        public virtual ApplicationUser User { get; set; }

        public IList<Record> Records { get; set; }

        public IList<ExerciseWorkout> ExerciseWorkouts { get; set; } = new List<ExerciseWorkout>();
    }
}
 
