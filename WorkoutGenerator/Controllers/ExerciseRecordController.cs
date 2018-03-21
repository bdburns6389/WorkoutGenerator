using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using WorkoutGenerator.Data;
using WorkoutGenerator.Models;
using WorkoutGenerator.ViewModels;

namespace WorkoutGenerator.Controllers
{
    public class ExerciseRecordController : Controller
    {
        private ApplicationDbContext context;

        public ExerciseRecordController(ApplicationDbContext dbContext)
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

        public IActionResult Add()
        {//Create form for each exercise to have sets reps and weight to submit
            string user = User.Identity.Name;
            ApplicationUser userLoggedIn = context.Users.Single(c => c.UserName == user);
            AddExerciseRecordViewModel addExerciseRecordViewModel = new AddExerciseRecordViewModel();
            return View(addExerciseRecordViewModel);
        }

        [HttpPost]
        public IActionResult Add(AddExerciseRecordViewModel addExerciseRecordViewModel)
        {//Create records of exercise sets reps and weights to be added to database.
            if (ModelState.IsValid)
            {
                string user = User.Identity.Name;
                ApplicationUser userLoggedIn = context.Users.Single(c => c.UserName == user);

                ExerciseRecord newRecord = new ExerciseRecord
                {
                    Sets = addExerciseRecordViewModel.Sets,
                    Reps = addExerciseRecordViewModel.Reps,
                    Weight = addExerciseRecordViewModel.Weight,
                    DateCreated = DateTime.Now,
                    OwnerId = userLoggedIn.Id,//TODO Not Sure if creation of newRecord is correct.
                    WorkoutID = addExerciseRecordViewModel.WorkoutID,
                    ExerciseID = addExerciseRecordViewModel.ExerciseID
                };
                context.ExerciseRecords.Add(newRecord);
                context.SaveChanges();

                return Redirect("/ExerciseRecord/ViewExerciseRecord" + newRecord.ID);
            }
            else
            {
                return View(addExerciseRecordViewModel);
            }
        }
    }
}