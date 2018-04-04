using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WorkoutGenerator.Models;

namespace WorkoutGenerator.ViewModels
{
    public class ViewRecordViewModel
    {
        public Workout Workout { get; set; }
        public IList<Record> Records { get; set; }
        public Record Record { get; set; }
        public List<ExerciseWorkout> ExerciseWorkouts { get; set; }
        public Exercise Exercise { get; set; }
    }
}
