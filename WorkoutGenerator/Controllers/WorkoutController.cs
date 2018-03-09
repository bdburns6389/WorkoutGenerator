using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RandomWorkout.Models;
using RandomWorkout.ViewModels;
using WorkoutGenerator.Data;
using WorkoutGenerator.Models;

namespace RandomWorkout.Controllers
{
    public class WorkoutController : Controller
    {
        private readonly ApplicationDbContext context;

        public WorkoutController(ApplicationDbContext dbContext)
        {
            context = dbContext;
        }

        //[AllowAnonymous]  THis will allow access without logging in while Global authorization is enabled
        //                  in Startup.cs .MVC()
        public IActionResult Index()
        {
            string user = User.Identity.Name;
            ApplicationUser userLoggedIn = context.Users.Single(c => c.UserName == user);
            var filteredWorkouts = context.Workouts.Where(c => c.OwnerId == userLoggedIn.Id).ToList();
            List<Workout> workouts = context.Workouts.ToList();
            return View(filteredWorkouts);
        }
        //[Authorize]  This attribute will redirect to login page to allow access.
        public IActionResult Add()
        {
            AddWorkoutViewModel addMenuViewModel = new AddWorkoutViewModel();
            return View(addMenuViewModel);
        }

        [HttpPost]
        public IActionResult Add(AddWorkoutViewModel addWorkoutViewModel)
        {
            if (ModelState.IsValid)
            {
                //Create user id connection to put into new exercise, linking ApplciationUser and Workout
                string user = User.Identity.Name;
                ApplicationUser userLoggedIn = context.Users.Single(c => c.UserName == user);

                Workout newWorkout = new Workout
                {
                    Name = addWorkoutViewModel.Name,
                    DateCreated = DateTime.Now,
                    OwnerId = userLoggedIn.Id
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

            Workout workout = context.Workouts.Single(m => m.ID == id); 

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
        {

            if (ModelState.IsValid)
            {
                var exerciseID = addWorkoutExerciseViewModel.ExerciseID;
                var workoutID = addWorkoutExerciseViewModel.WorkoutID;

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
                }
                return Redirect(string.Format("/Workout/ViewWorkout/{0}", addWorkoutExerciseViewModel.WorkoutID));
            } 
            return View(addWorkoutExerciseViewModel);
        }
    }
}