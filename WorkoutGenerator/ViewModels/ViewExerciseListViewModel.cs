using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WorkoutGenerator.Models;

namespace WorkoutGenerator.ViewModels
{
    public class ViewExerciseListViewModel
    {
        public Workout Workout { get; set; }
        public List<Exercise> Exercises { get; set;}


        public ViewExerciseListViewModel() { }
    }
}
