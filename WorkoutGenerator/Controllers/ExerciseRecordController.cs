using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using RandomWorkout.Models;
using WorkoutGenerator.Data;
using WorkoutGenerator.Models;

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

    }
}