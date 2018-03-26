using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using WorkoutGenerator.Models;

namespace WorkoutGenerator.ViewModels
{
    public class AddWorkoutExerciseViewModel
    {
        public Workout Workout { get; set; }
        public List<SelectListItem> Exercises { get; set; }

        public int WorkoutID { get; set; }
        [Display(Name = "Exercise")]//Add to make list Not say CheeseID(Not a great look)
        public int ExerciseID { get; set; }


        public AddWorkoutExerciseViewModel() { }

        public AddWorkoutExerciseViewModel(Workout workout, IEnumerable<Exercise> exercises)
        {
            Exercises = new List<SelectListItem>();

            foreach (var exercise in exercises)
            {
                Exercises.Add(new SelectListItem
                {
                    Value = exercise.ExerciseID.ToString(),
                    Text = exercise.Name
                });
            }
            Workout = workout;
        }
    }
}