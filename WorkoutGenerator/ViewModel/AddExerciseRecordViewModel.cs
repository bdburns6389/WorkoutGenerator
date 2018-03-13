﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using RandomWorkout.Models;

namespace WorkoutGenerator.ViewModel
{
    public class AddExerciseRecordViewModel
    {
        [Required(ErrorMessage = "Please enter number of sets")]
        public string Sets { get; set; }

        [Required(ErrorMessage = "Please enter number of reps")]
        public string Reps { get; set; }

        [Required(ErrorMessage = "Please enter amount of weight")]
        public string Weight { get; set; }

        //Date Created
        public DateTime DateCreated { get; set; }
        //Link to user
        public string OwnerId { get; set; }
        //Link to Exercise
        public string ExerciseID { get; set; }
        //Link to Workout
        public string WorkoutID { get; set; }
        
        public AddExerciseRecordViewModel()
        {

        }
    }
}
