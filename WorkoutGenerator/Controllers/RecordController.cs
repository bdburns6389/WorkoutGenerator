using System;
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
    public class RecordController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly UserManager<ApplicationUser> _userManager;

        public RecordController(IConfiguration config, UserManager<ApplicationUser> userManger)
        {
            _configuration = config;
            _userManager = userManger;
        }

        public IActionResult Index()
        {
            var userId = _userManager.GetUserId(User);
            using (IDbConnection db = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                const string sql = "SELECT * FROM Workouts WHERE OwnerId = @userId";
                var filterWorkouts = db.Query<Workout>(sql, new { userId }).ToList();
                return View(filterWorkouts);
            }
        }

        public IActionResult ExerciseList(int id)
        {
            var userId = _userManager.GetUserId(User);
            using (IDbConnection db = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                const string exerciseSql = "SELECT Exercises.ExerciseId, Exercises.Name, Exercises.Description FROM Exercises" +
                                   " JOIN ExerciseWorkouts ON Exercises.ExerciseId = ExerciseWorkouts.ExerciseId" +
                                   " JOIN Workouts ON ExerciseWorkouts.WorkoutId = Workouts.WorkoutId" +
                                   " WHERE ExerciseWorkouts.WorkoutId = @id AND Workouts.OwnerId = @userId";
                var exercise = db.Query<Exercise>(exerciseSql, new { id, userId }).ToList();
                const string workoutSql = "SELECT * FROM Workouts WHERE WorkoutId = @id";
                var workout = db.Query<Workout>(workoutSql, new { id }).FirstOrDefault();
                var viewModel = new ViewExerciseListViewModel
                {
                    Workout = workout,
                    Exercises = exercise
                };
                return View(viewModel);
            }
        }

        public IActionResult AddRecord(int exerciseId, int workoutId)
        {
            using (IDbConnection db = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                const string workoutSql = "SELECT * FROM Workouts WHERE WorkoutID = @workoutId";
                var workout = db.Query<Workout>(workoutSql, new { workoutId }).FirstOrDefault();

                const string exerciseSql = "SELECT * FROM Exercises WHERE ExerciseID = @exerciseId";
                var exercise = db.Query<Exercise>(exerciseSql, new { exerciseId }).FirstOrDefault();

                var viewModel = new AddRecordViewModel
                {
                    Workout = workout,
                    Exercise = exercise
                };

                return View(viewModel);
            }
        }

        [HttpPost]
        public IActionResult AddRecord(AddRecordViewModel addRecordViewModel, int exerciseId, int workoutId)
        {
            if (!ModelState.IsValid) return View(addRecordViewModel);

            var userId = _userManager.GetUserId(User);
            using (IDbConnection db = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                const string sql = "INSERT INTO Records (Sets, Reps, Weight, DateCreated, OwnerId, WorkoutID, FK_ExerciseID)" +
                                   " VALUES(@sets, @reps, @weight, @dateCreated, @ownerId, @workoutId, @FK_ExerciseID);";

                db.Execute(sql, new
                {
                    sets = addRecordViewModel.Sets,
                    reps = addRecordViewModel.Reps,
                    weight = addRecordViewModel.Weight,
                    dateCreated = DateTime.Now,
                    ownerId = userId,
                    workoutId,
                    FK_ExerciseID = exerciseId
                });
            }

            return Redirect("/Record/ExerciseList/" + workoutId);
        }

        public IActionResult RecordIndexWorkout()
        {
            var userId = _userManager.GetUserId(User);
            using (IDbConnection db = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                const string sql = "SELECT * FROM Workouts WHERE OwnerId = @userId";
                var filteredWorkouts = db.Query<Workout>(sql, new { userId });
                return View(filteredWorkouts);
            }
        }

        public IActionResult RecordIndexExercise(int id)
        {
            var userId = _userManager.GetUserId(User);
            using (IDbConnection db = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                const string exerciseSql = "SELECT Exercises.ExerciseId, Exercises.Name, Exercises.Description FROM Exercises" +
                                           " JOIN ExerciseWorkouts ON Exercises.ExerciseId = ExerciseWorkouts.ExerciseId" +
                                           " JOIN Workouts ON ExerciseWorkouts.WorkoutId = Workouts.WorkoutId" +
                                           " WHERE ExerciseWorkouts.WorkoutId = @id AND Workouts.OwnerId = @userId";
                var exercises = db.Query<Exercise>(exerciseSql, new { id, userId }).ToList();

                const string workoutSql = "SELECT * FROM Workouts WHERE WorkoutId = @id";
                var workout = db.Query<Workout>(workoutSql, new { id }).FirstOrDefault();

                var viewModel = new ViewRecordViewModel
                {
                    Workout = workout,
                    Exercises = exercises
                };
                return View(viewModel);
            }
        }

        public IActionResult ViewRecords(int workoutId, int exerciseId)
        {
            using (IDbConnection db = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                const string exerciseSql = "SELECT * FROM Exercises WHERE ExerciseID = @exerciseId";
                var exercise = db.Query<Exercise>(exerciseSql, new { exerciseId }).FirstOrDefault();

                const string recordsSql = "SELECT * FROM Records WHERE FK_ExerciseID = @exerciseId AND WorkoutID = @workoutId";
                var records = db.Query<Record>(recordsSql, new { exerciseId, workoutId }).ToList();

                var viewModel = new ViewRecordViewModel
                {
                    Exercise = exercise,
                    Records = records
                };
                return View(viewModel);
            }
        }

        public IActionResult Remove()
        {
            var userId = _userManager.GetUserId(User);
            ViewBag.title = "Remove Records";
            using (IDbConnection db = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                const string sql = "SELECT * FROM Records WHERE OwnerId = @userId";
                var records = db.Query<Record>(sql, new { userId });
                ViewBag.records = records;
                return View();
            }
        }

        [HttpPost]
        public IActionResult Remove(int[] recordIds)
        {
            using (IDbConnection db = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                foreach (var recordId in recordIds)
                {
                    const string sql = "DELETE FROM Records WHERE RecordID = @recordId";
                    db.Execute(sql, new { recordId });
                }

                return Redirect("/");
            }
        }
    }
}