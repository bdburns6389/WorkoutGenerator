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

        public IActionResult Add(int id)
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
        public IActionResult Add(AddRecordViewModel addRecordViewModel)
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
                    OwnerId = userLoggedIn.Id,//TODO Not Sure if creation of newRecord is correct.
                    WorkoutID = addRecordViewModel.WorkoutID
                };
                context.Records.Add(newRecord);
                context.SaveChanges();

                return Redirect("/Record/Index");
            }
            else
            {
                return View(addRecordViewModel);
            }
        }
    }
}