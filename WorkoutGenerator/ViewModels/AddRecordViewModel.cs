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

        //Date Created
        public DateTime DateCreated { get; set; }
        //Link to user
        public string OwnerId { get; set; }
        //Link to Exercise
        public int ExerciseID { get; set; }
        //Link to Workout
        public int WorkoutID { get; set; }

        public IList<ExerciseWorkout> Exercises { get; set; }
    }
}

