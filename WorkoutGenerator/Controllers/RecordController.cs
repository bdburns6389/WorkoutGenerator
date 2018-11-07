using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WorkoutGenerator.Data;
using WorkoutGenerator.Models;
using WorkoutGenerator.ViewModels;

namespace WorkoutGenerator.Controllers
{
    public class RecordController : Controller
    {
        private ApplicationDbContext context;

        public RecordController(ApplicationDbContext dbContext)
        {
            context = dbContext;
        }

        public IActionResult Index()
        {
            string user = User.Identity.Name;
            ApplicationUser userLoggedIn = context.Users.Single(c => c.UserName == user);
            var filteredWorkouts = context.Workouts.Where(c => c.OwnerId == userLoggedIn.Id).ToList();
            return View(filteredWorkouts);
        }

        public IActionResult ExerciseList(int id)
        {
            string user = User.Identity.Name;
            ApplicationUser userLoggedIn = context.Users.Single(c => c.UserName == user);
            List<ExerciseWorkout> exercises = context
                .ExerciseWorkouts
                .Include(item => item.Exercise)
                .Where(cm => cm.WorkoutID == id && cm.Workout.OwnerId == userLoggedIn.Id)//cm.Workout.OwnerId == userLoggedIn.Id returns list of owner specific workouts
                .ToList();

            Workout workout = context.Workouts.Single(m => m.WorkoutID == id); //Only needed for title in page and to link to add an exercise

            ViewExerciseListViewModel viewModel = new ViewExerciseListViewModel
            {
                Workout = workout,
                ExerciseWorkouts = exercises
            };

            return View(viewModel);
        }

        public IActionResult AddRecord(int ExerciseID, int WorkoutID)
        {
            string user = User.Identity.Name;
            ApplicationUser userLoggedIn = context.Users.Single(c => c.UserName == user);

            List<ExerciseWorkout> exercises = context
                .ExerciseWorkouts
                .Include(item => item.Exercise)
                .Where(cm => cm.WorkoutID == ExerciseID && cm.Workout.OwnerId == userLoggedIn.Id)//cm.Workout.OwnerId == userLoggedIn.Id returns list of owner specific workouts
                .ToList();

            Workout workout = context.Workouts.Single(m => m.WorkoutID == WorkoutID);

            Exercise exercise = context.Exercises.Single(m => m.ExerciseID == ExerciseID);

            AddRecordViewModel viewModel = new AddRecordViewModel
            {
                Workout = workout,
                Exercises = exercises,
                Exercise = exercise
            };

            return View(viewModel);
        }

        [HttpPost]
        public IActionResult AddRecord(AddRecordViewModel addRecordViewModel, int ExerciseID, int WorkoutID)
        {//Create records of exercise sets reps and weights to be added to database.
            if (ModelState.IsValid)
            {
                string user = User.Identity.Name;
                ApplicationUser userLoggedIn = context.Users.Single(c => c.UserName == user);
                Record newRecord = new Record
                {
                    Sets = addRecordViewModel.Sets,
                    Reps = addRecordViewModel.Reps,
                    Weight = addRecordViewModel.Weight,
                    DateCreated = DateTime.Now,
                    OwnerId = userLoggedIn.Id,
                    WorkoutID = WorkoutID,
                    FK_ExerciseID = ExerciseID
                };
                context.Records.Add(newRecord);
                context.SaveChanges();

                return Redirect("/Record/ExerciseList/" + WorkoutID);
            }
            else
            {
                return View(addRecordViewModel);
            }
        }

        public IActionResult RecordIndexWorkout()
        {
            string user = User.Identity.Name;
            ApplicationUser userLoggedIn = context
                .Users
                .Single(c => c.UserName == user);

            List<Workout> workouts = context
                .Workouts
                .ToList();

            var filteredWorkouts = context
                .Workouts
                .Where(c => c.OwnerId == userLoggedIn.Id).ToList();

            return View(filteredWorkouts);
        }

        public IActionResult RecordIndexExercise(int id)
        {
            string user = User.Identity.Name;
            ApplicationUser userLoggedIn = context.Users.Single(c => c.UserName == user);

            List<ExerciseWorkout> exercises = context
                .ExerciseWorkouts
                .Include(item => item.Exercise)
                .Where(cm => cm.WorkoutID == id && cm.Workout.OwnerId == userLoggedIn.Id)//cm.Workout.OwnerId == userLoggedIn.Id returns list of owner specific workouts
                .ToList();

            Workout workout = context
                .Workouts
                .Single(m => m.WorkoutID == id); //Only needed for title in page and to link to add an exercise

            ViewRecordViewModel viewModel = new ViewRecordViewModel
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
            string user = User.Identity.Name;
            ApplicationUser userLoggedIn = context.Users.Single(c => c.UserName == user);
            ViewBag.title = "Remove Records";
            ViewBag.records = context.Records.Where(c => c.OwnerId == userLoggedIn.Id).ToList();
            return View();
        }

        [HttpPost]
        public IActionResult Remove(int[] recordIds)
        {
            foreach (int recordId in recordIds)
            {
                Record theRecord = context.Records.Single(c => c.RecordID == recordId);
                context.Records.Remove(theRecord);
            }

            context.SaveChanges();

            return Redirect("/");
        }
    }
}