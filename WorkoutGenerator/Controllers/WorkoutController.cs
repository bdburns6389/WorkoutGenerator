using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RandomWorkout.Models;
using RandomWorkout.ViewModels;
using WorkoutGenerator.Data;

namespace RandomWorkout.Controllers
{
    public class WorkoutController : Controller
    {
        private readonly ApplicationDbContext context;

        public WorkoutController(ApplicationDbContext dbContext)
        {
            context = dbContext;
        }


        public IActionResult Index()
        {//Might be wrong context.  Might need to be context.Menus.
            List<Workout> workouts = context.Workouts.ToList();
            return View(workouts);
        }

        public IActionResult Add()
        {//Won't work yet due to no menuviewmodel.
            AddWorkoutViewModel addMenuViewModel = new AddWorkoutViewModel();
            return View(addMenuViewModel);
        }

        [HttpPost]
        public IActionResult Add(AddWorkoutViewModel addWorkoutViewModel)
        {
            if (ModelState.IsValid)
            {
                // Add the new cheese to my existing cheeses
                Workout newWorkout = new Workout
                {
                    Name = addWorkoutViewModel.Name,
                };

                context.Workouts.Add(newWorkout);
                context.SaveChanges();

                return Redirect("/Workout/ViewWorkout/" + newWorkout.ID);
            }

            return View(addWorkoutViewModel);
        }

        public IActionResult ViewWorkout(int id)
        {
            List<ExerciseWorkout> exercises = context
                .ExerciseWorkouts
                .Include(item => item.Exercise)
                .Where(cm => cm.WorkoutID == id)
                .ToList();

            Workout workout = context.Workouts.Single(m => m.ID == id); //TODO receiving contains no elements error.

            ViewWorkoutViewModel viewModel = new ViewWorkoutViewModel
            {
                Workout = workout,
                Exercises = exercises
            };

            return View(viewModel);
        }

        public IActionResult AddExercise(int id)
        {
            Workout workout = context.Workouts.Single(m => m.ID == id);
            List<Exercise> exercises = context.Exercises.ToList();
            return View(new AddWorkoutExerciseViewModel(workout, exercises));
        }

        [HttpPost]
        public IActionResult AddExercise(AddWorkoutExerciseViewModel addWorkoutExerciseViewModel)
        {//Changed from AddItem to work for Exercise program

            if (ModelState.IsValid)
            {
                var exerciseID = addWorkoutExerciseViewModel.ExerciseID;
                var workoutID = addWorkoutExerciseViewModel.WorkoutID;
                //WORK HERE!!!!!!!!!!!!!!!!!!!!!!!!!!!!
                IList<ExerciseWorkout> existingItems = context.ExerciseWorkouts
                    .Where(cm => cm.ExerciseID == exerciseID)
                    .Where(cm => cm.WorkoutID == workoutID).ToList();

                if (existingItems.Count == 0)
                {
                    ExerciseWorkout workoutItem = new ExerciseWorkout
                    {
                        Exercise = context.Exercises.Single(e => e.ID == exerciseID),
                        Workout = context.Workouts.Single(w => w.ID == workoutID)
                    };

                    context.ExerciseWorkouts.Add(workoutItem);
                    context.SaveChanges();
                    //TODO  Adding cheese to both menus for some reason.
                    //TODO not sure where to redirect.
                }
                return Redirect(string.Format("/Workout/ViewWorkout/{0}", addWorkoutExerciseViewModel.WorkoutID));
            }  //Should this Be .MenuID?  Or something else?
            return View(addWorkoutExerciseViewModel);
        }
    }
}