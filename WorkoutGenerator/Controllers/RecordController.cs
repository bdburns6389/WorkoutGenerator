using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
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

        public IActionResult Add()
        {//Create form for each exercise to have sets reps and weight to submit
            string user = User.Identity.Name;
            ApplicationUser userLoggedIn = context.Users.Single(c => c.UserName == user);
            AddRecordViewModel addRecordViewModel = new AddRecordViewModel();
            return View(addRecordViewModel);
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
                    WorkoutID = addRecordViewModel.WorkoutID,
                };
                context.Records.Add(newRecord);
                context.SaveChanges();

                return Redirect("/Record/ViewRecord" + newRecord.ID);
            }
            else
            {
                return View(addRecordViewModel);
            }
        }
    }
}