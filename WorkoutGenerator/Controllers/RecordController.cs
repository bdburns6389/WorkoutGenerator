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
            List<Workout> workouts = context.Workouts.ToList();
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

        public IActionResult AddRecord(int id)
        {//Create form for each exercise to have sets reps and weight to submit
            //!!!!!!!!!!!!!!TAKEN FROM WORKOUT CONTROLLER!!!!!!!!!  MAY NEED CHANGING!!!!!!!!!!!!!!!!
            string user = User.Identity.Name;
            ApplicationUser userLoggedIn = context.Users.Single(c => c.UserName == user);
            List<ExerciseWorkout> exercises = context
                .ExerciseWorkouts
                .Include(item => item.Exercise)
                .Where(cm => cm.WorkoutID == id && cm.Workout.OwnerId == userLoggedIn.Id)//cm.Workout.OwnerId == userLoggedIn.Id returns list of owner specific workouts
                .ToList();

            Workout workout = context.Workouts.Single(m => m.WorkoutID == id);

            AddRecordViewModel viewModel = new AddRecordViewModel
            {
                Workout = workout,
                Exercises = exercises
            };

            return View(viewModel);
        }

        [HttpPost]
        public IActionResult AddRecord(AddRecordViewModel addRecordViewModel, int id)
        {//Create records of exercise sets reps and weights to be added to database.
            if (ModelState.IsValid)
            {
                string user = User.Identity.Name;
                ApplicationUser userLoggedIn = context.Users.Single(c => c.UserName == user);
                //exercises hopefully returns list of exercises from 'int id' parameter,
                //which can then be used to iterate over each exercise put into record table
                List<ExerciseWorkout> exercises = context
                .ExerciseWorkouts
                .Include(item => item.Exercise)
                .Where(cm => cm.WorkoutID == id && cm.Workout.OwnerId == userLoggedIn.Id)
                .ToList();
                foreach (var exercise in exercises)
                {
                    Record newRecord = new Record
                    {
                        Sets = addRecordViewModel.Sets,
                        Reps = addRecordViewModel.Reps,
                        Weight = addRecordViewModel.Weight,
                        DateCreated = DateTime.Now,//TODO Make this show only day not time of day
                        OwnerId = userLoggedIn.Id,//TODO Not Sure if creation of newRecord is correct.
                        WorkoutID = addRecordViewModel.Workout.WorkoutID,
                        FK_ExerciseID = addRecordViewModel.Exercise.ExerciseID//TODO ExerciseID not entering into table.
                    };
                    context.Records.Add(newRecord);
                    context.SaveChanges();
                }
                return Redirect("/Record/Index");
            }
            else
            {
                return View(addRecordViewModel);
            }
        }

        public IActionResult RecordIndex()
        {
            string user = User.Identity.Name;
            ApplicationUser userLoggedIn = context.Users.Single(c => c.UserName == user);
            var filteredWorkouts = context.Workouts.Where(c => c.OwnerId == userLoggedIn.Id).ToList();
            List<Workout> workouts = context.Workouts.ToList();
            return View(filteredWorkouts);
        }

        public IActionResult ViewRecords(int id)
        {//TODO #1  Make ViewRecords return list of all exercise records ordered by recent to old
            string user = User.Identity.Name;
            ApplicationUser userLoggedIn = context.Users.Single(c => c.UserName == user);
            List<ExerciseWorkout> exercises = context
                .ExerciseWorkouts
                .Include(item => item.Exercise)
                .Where(cm => cm.WorkoutID == id && cm.Workout.OwnerId == userLoggedIn.Id)//cm.Workout.OwnerId == userLoggedIn.Id returns list of owner specific workouts
                .ToList();

            Workout workout = context.Workouts.Single(m => m.WorkoutID == id); //Only needed for title in page and to link to add an exercise

            List<Record> records = context
                .Records
                .Include(item => item.Reps)
                .Where(c => c.WorkoutID == id)
                .ToList();
            //TODO List<Record> not working correctly
            ViewRecordViewModel viewModel = new ViewRecordViewModel
            {
                Workout = workout,
                Exercises = exercises,
                Records = records
            };

            return View(viewModel);
        }
    }
}