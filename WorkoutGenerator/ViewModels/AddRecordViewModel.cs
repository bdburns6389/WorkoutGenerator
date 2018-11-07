using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using WorkoutGenerator.Models;

namespace WorkoutGenerator.ViewModels
{
    public class AddRecordViewModel
    {
        [Required(ErrorMessage = "Please enter number of sets")]
        public string Sets { get; set; }

        [Required(ErrorMessage = "Please enter number of reps")]
        public string Reps { get; set; }

        [Required(ErrorMessage = "Please enter amount of weight")]
        public string Weight { get; set; }

        public IList<ExerciseWorkout> Exercises { get; set; }

        public Workout Workout { get; set; }

        public Exercise Exercise { get; set; }
    }
}

