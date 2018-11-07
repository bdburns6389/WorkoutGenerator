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
        private readonly ApplicationDbContext context;
        private readonly IConfiguration _configuration;
        private readonly UserManager<ApplicationUser> _userManager;

        public RecordController(ApplicationDbContext dbContext, IConfiguration config, UserManager<ApplicationUser> userManger)
        {
            context = dbContext;
            _configuration = config;
            _userManager = userManger;
        }

        public IActionResult Index()
        {
            var userId = _userManager.GetUserId(User);
            using (IDbConnection db = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                const string sql = "SELECT * FROM Workouts WHERE OwnerId = @userId";
                var filterWorkouts = db.Query<Workout>(sql, new {userId}).ToList();
                return View(filterWorkouts);
            }
        }

        public IActionResult ExerciseList(int id)
        {
            var userId = _userManager.GetUserId(User);
            List<ExerciseWorkout> exercises = context
                .ExerciseWorkouts
                .Include(item => item.Exercise)
                .Where(cm => cm.WorkoutID == id && cm.Workout.OwnerId == userId)//cm.Workout.OwnerId == userLoggedIn.Id returns list of owner specific workouts
                .ToList();

            Workout workout = context.Workouts.Single(m => m.WorkoutID == id); //Only needed for title in page and to link to add an exercise

            ViewExerciseListViewModel viewModel = new ViewExerciseListViewModel
            {
                Workout = workout,
                ExerciseWorkouts = exercises
            };

            return View(viewModel);
        }

        public IActionResult AddRecord(int exerciseId, int workoutId)
        {
            var userId = _userManager.GetUserId(User);

            List<ExerciseWorkout> exercises = context
                .ExerciseWorkouts
                .Include(item => item.Exercise)
                .Where(cm => cm.WorkoutID == exerciseId && cm.Workout.OwnerId == userId)//cm.Workout.OwnerId == userLoggedIn.Id returns list of owner specific workouts
                .ToList();

            Workout workout = context.Workouts.Single(m => m.WorkoutID == workoutId);

            Exercise exercise = context.Exercises.Single(m => m.ExerciseID == exerciseId);

            AddRecordViewModel viewModel = new AddRecordViewModel
            {
                Workout = workout,
                Exercises = exercises,
                Exercise = exercise
            };

            return View(viewModel);
        }

        [HttpPost]
        public IActionResult AddRecord(AddRecordViewModel addRecordViewModel, int exerciseId, int workoutId)
        {
            if (!ModelState.IsValid) return View(addRecordViewModel);

            var userId = _userManager.GetUserId(User);
            using (IDbConnection db = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                var sql =
                    "INSERT INTO Records (Sets, Reps, Weight, DateCreated, OwnerId, WorkoutID, FK_ExerciseID)" +
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

            var filteredWorkouts = context
                .Workouts
                .Where(c => c.OwnerId == userId).ToList();

            return View(filteredWorkouts);
        }

        public IActionResult RecordIndexExercise(int id)
        {
            var userId = _userManager.GetUserId(User);

            var exercises = context
                .ExerciseWorkouts
                .Include(item => item.Exercise)
                .Where(cm => cm.WorkoutID == id && cm.Workout.OwnerId == userId)//cm.Workout.OwnerId == userLoggedIn.Id returns list of owner specific workouts
                .ToList();

            var workout = context
                .Workouts
                .Single(m => m.WorkoutID == id); //Only needed for title in page and to link to add an exercise

            var viewModel = new ViewRecordViewModel
            {
                Workout = workout,
                ExerciseWorkouts = exercises
            };

            return View(viewModel);
        }

        public IActionResult ViewRecords(int WorkoutID, int ExerciseID)
        {
            string user = User.Identity.Name;
            ApplicationUser userLoggedIn = context
                .Users
                .Single(c => c.UserName == user);
            
            Exercise exercise = context
                .Exercises
                .Single(c => c.ExerciseID == ExerciseID);

            List<Record> records = context
                .Records
                .Include(c => c.ExerciseRecords)
                .Where(c => c.FK_ExerciseID == ExerciseID && c.WorkoutID == WorkoutID)
                .ToList();
            
            ViewRecordViewModel viewModel = new ViewRecordViewModel
            {
                Exercise = exercise,
                Records = records
            };

            return View(viewModel);
        }

        public IActionResult Remove()
        {
            var userId = _userManager.GetUserId(User);
            ViewBag.title = "Remove Records";
            ViewBag.records = context.Records.Where(c => c.OwnerId == userId).ToList();
            return View();
        }

        [HttpPost]
        public IActionResult Remove(int[] recordIds)
        {
            using (IDbConnection db = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                foreach (int recordId in recordIds)
                {
                    const string sql = "DELETE FROM Records WHERE RecordID = @recordId";
                    db.Execute(sql, new { recordId });
                }

                return Redirect("/");
            }
        }
    }
}