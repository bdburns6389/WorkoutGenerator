using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Dapper;
using Microsoft.AspNetCore.Identity;
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
        private readonly IConfiguration _configuration;
        private readonly UserManager<ApplicationUser> _userManager;


        public WorkoutController(IConfiguration config, UserManager<ApplicationUser> userManager)
        {
            _configuration = config;
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            var userId = _userManager.GetUserId(User);
            using (IDbConnection db = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                const string sql = "SELECT * FROM Workouts WHERE OwnerId = @userId";
                var filteredWorkouts = db.Query<Workout>(sql, new { userId }).ToList();
                return View(filteredWorkouts);
            }
        }

        public IActionResult Add()
        {
            var addMenuViewModel = new AddWorkoutViewModel();
            return View(addMenuViewModel);
        }

        [HttpPost]
        public IActionResult Add(AddWorkoutViewModel addWorkoutViewModel)
        {
            if (!ModelState.IsValid) return View(addWorkoutViewModel);

            var userId = _userManager.GetUserId(User);
            var currentTime = DateTime.Now;
            using (IDbConnection db = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                const string sql =
                    @"INSERT INTO Workouts (Name, DateCreated, OwnerId) VALUES (@Name, @DateCreated, @OwnerId);
                      SELECT SCOPE_IDENTITY()";
                var id = db.Query<int>(sql, new {Name = addWorkoutViewModel.Name, DateCreated = currentTime, OwnerId = userId}).Single();
                return Redirect($"/Workout/ViewWorkout/{id}");
            }
        }

        public IActionResult ViewWorkout(int id)
        {
            var userId = _userManager.GetUserId(User);
           
            using (IDbConnection db = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                const string sql =
                    "SELECT Exercises.*, Workouts.WorkoutID FROM Exercises" +
                    " JOIN ExerciseWorkouts ON ExerciseWorkouts.ExerciseID = Exercises.ExerciseID"+
                    " JOIN Workouts ON ExerciseWorkouts.WorkoutID = Workouts.WorkoutID"+
                    " WHERE ExerciseWorkouts.WorkoutID = @id AND Workouts.OwnerID = @userId";
                var exercises = db.Query<Exercise>(sql, new {id, userId}).ToList();

                const string workoutSql = "SELECT * FROM Workouts WHERE WorkoutID = @id";
                var workout = db.Query<Workout>(workoutSql, new {id}).FirstOrDefault();

                var viewModel = new ViewWorkoutViewModel
                {
                    Workout = workout,
                    Exercises = exercises
                };
                return View(viewModel);
            }
        }

        public IActionResult AddExercise(int id)
        {
            var userId = _userManager.GetUserId(User);
            using (IDbConnection db = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                const string workoutSql = "SELECT * FROM Workouts WHERE WorkoutId = @id";
                var workout = db.Query<Workout>(workoutSql, new {id}).FirstOrDefault();

                const string exercisesSql = "SELECT * FROM Exercises WHERE OwnerId = @userId";
                var exercises = db.Query<Exercise>(exercisesSql, new {userId});

                return View(new AddWorkoutExerciseViewModel(workout, exercises));
            }
        }

        [HttpPost]
        public IActionResult AddExercise(AddWorkoutExerciseViewModel addWorkoutExerciseViewModel)
        {
            if (!ModelState.IsValid) return View(addWorkoutExerciseViewModel);

            var exerciseId = addWorkoutExerciseViewModel.ExerciseID;
            var workoutId = addWorkoutExerciseViewModel.WorkoutID;

            List<ExerciseWorkout> doesItemExist;
            using (IDbConnection db = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                const string sql =
                    "SELECT * FROM ExerciseWorkouts WHERE ExerciseId = @exerciseId AND WorkoutId = @workoutId";
                doesItemExist = db.Query<ExerciseWorkout>(sql, new {exerciseId, workoutId}).ToList();
            }

            if (doesItemExist.Count != 0)
                return Redirect($"/Workout/ViewWorkout/{addWorkoutExerciseViewModel.WorkoutID}");
            
            using (IDbConnection db = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                const string exerciseSql = "SELECT * FROM Exercises WHERE ExerciseID = @exerciseId";
                var exercise = db.Query<Exercise>(exerciseSql, new {exerciseId}).FirstOrDefault();

                const string workoutSql = "SELECT * FROM Workouts WHERE WorkoutID = @workoutId";
                var workout = db.Query<Workout>(workoutSql, new {workoutId}).FirstOrDefault();

                const string insertExerciseSql = "INSERT INTO ExerciseWorkouts (ExerciseId, WorkoutId) VALUES (@exerciseId, @workoutId);";
                db.Execute(insertExerciseSql, new {exerciseId = exercise.ExerciseID, workoutId = workout.WorkoutID});

                return Redirect($"/Workout/ViewWorkout/{addWorkoutExerciseViewModel.WorkoutID}");
            }
        }

        public IActionResult Remove()
        {
            var userId = _userManager.GetUserId(User);
            ViewBag.title = "Remove Workouts";
            using (IDbConnection db = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                const string sql = "SELECT * FROM Workouts WHERE OwnerId = @userId";
                var workouts = db.Query<Workout>(sql, new {userId});

                ViewBag.workouts = workouts;
                return View();
            }
        }

        [HttpPost]
        public IActionResult Remove(int[] workoutIds)
        {
            using (IDbConnection db = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                foreach (var workoutId in workoutIds)
                {
                    const string sql = "DELETE FROM Workouts WHERE WorkoutId = @workoutId";
                    db.Execute(sql, new {workoutId});
                }
            }
            return Redirect("/");
        }
    }
}