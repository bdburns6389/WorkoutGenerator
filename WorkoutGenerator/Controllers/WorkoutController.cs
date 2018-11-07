using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Dapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using WorkoutGenerator.Data;
using WorkoutGenerator.Models;
using WorkoutGenerator.ViewModels;

namespace WorkoutGenerator.Controllers
{
    public class WorkoutController : Controller
    {
        private readonly ApplicationDbContext context;
        private readonly IConfiguration _configuration;

        public WorkoutController(ApplicationDbContext dbContext, IConfiguration config)
        {
            context = dbContext;
            _configuration = config;
        }

        public IActionResult Index()
        {
            var user = User.Identity.Name;
            var userLoggedIn = context.Users.Single(c => c.UserName == user);
            using (IDbConnection db = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                const string sql = "SELECT * FROM Workouts WHERE OwnerId = @userLoggedIn";
                var filteredWorkouts = db.Query<Workout>(sql, new { userLoggedIn }).ToList();
                return View(filteredWorkouts);
            }
        }

        public IActionResult Add()
        {
            AddWorkoutViewModel addMenuViewModel = new AddWorkoutViewModel();
            return View(addMenuViewModel);
        }

        [HttpPost]
        public IActionResult Add(AddWorkoutViewModel addWorkoutViewModel)
        {
            if (!ModelState.IsValid) return View(addWorkoutViewModel);

            var user = User.Identity.Name;
            var userLoggedIn = context.Users.Single(c => c.UserName == user);

            var newWorkout = new Workout
            {
                Name = addWorkoutViewModel.Name,
                DateCreated = DateTime.Now,
                OwnerId = userLoggedIn.Id
            };

            context.Workouts.Add(newWorkout);
            context.SaveChanges();

            return Redirect($"/Workout/ViewWorkout/{newWorkout.WorkoutID}");

        }

        public IActionResult ViewWorkout(int id)
        {
            var user = User.Identity.Name;
            var userLoggedIn = context.Users.Single(c => c.UserName == user);
            var exercises = context
                .ExerciseWorkouts
                .Include(item => item.Exercise)
                .Where(cm => cm.WorkoutID == id && cm.Workout.OwnerId == userLoggedIn.Id)//cm.Workout.OwnerId == userLoggedIn.Id returns list of owner specific workouts
                .ToList();

            var workout = context.Workouts.Single(m => m.WorkoutID == id); //Only needed for title in page and to link to add an exercise

            var viewModel = new ViewWorkoutViewModel
            {
                Workout = workout,
                Exercises = exercises
            };

            return View(viewModel);
        }

        public IActionResult AddExercise(int id)
        {
            var user = User.Identity.Name;
            var userLoggedIn = context.Users.Single(c => c.UserName == user);

            var workout = context.Workouts.Single(m => m.WorkoutID == id);
            var exercises = context.Exercises.Where(c => c.OwnerId == userLoggedIn.Id).ToList();//OwnerId specifies user for exercise list.

            return View(new AddWorkoutExerciseViewModel(workout, exercises));
        }

        [HttpPost]
        public IActionResult AddExercise(AddWorkoutExerciseViewModel addWorkoutExerciseViewModel)
        {
            if (!ModelState.IsValid) return View(addWorkoutExerciseViewModel);

            var exerciseID = addWorkoutExerciseViewModel.ExerciseID;
            var workoutID = addWorkoutExerciseViewModel.WorkoutID;

            var existingItems = context.ExerciseWorkouts
                .Where(cm => cm.ExerciseID == exerciseID)
                .Where(cm => cm.WorkoutID == workoutID).ToList();

            if (existingItems.Count != 0)
                return Redirect($"/Workout/ViewWorkout/{addWorkoutExerciseViewModel.WorkoutID}");

            var workoutItem = new ExerciseWorkout
            {
                Exercise = context.Exercises.Single(e => e.ExerciseID == exerciseID),
                Workout = context.Workouts.Single(w => w.WorkoutID == workoutID)
            };

            context.ExerciseWorkouts.Add(workoutItem);
            context.SaveChanges();
            return Redirect($"/Workout/ViewWorkout/{addWorkoutExerciseViewModel.WorkoutID}");
        }

        public IActionResult Remove()
        {
            string user = User.Identity.Name;
            ApplicationUser userLoggedIn = context.Users.Single(c => c.UserName == user);
            ViewBag.title = "Remove Workouts";
            ViewBag.workouts = context.Workouts.Where(c => c.OwnerId == userLoggedIn.Id).ToList();
            return View();
        }

        [HttpPost]
        public IActionResult Remove(int [] workoutIds)
        {
            foreach (int workoutId in workoutIds)
            {
                Workout theWorkout = context.Workouts.Single(c => c.WorkoutID == workoutId);
                context.Workouts.Remove(theWorkout);
            }

            context.SaveChanges();

            return Redirect("/");
        }
    }
}