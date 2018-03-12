using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using RandomWorkout.Models;
using RandomWorkout.ViewModels;
using WorkoutGenerator.Data;
using WorkoutGenerator.Models;

namespace RandomWorkout.Controllers
{
    public class MuscleGroupController : Controller
    {
        private readonly ApplicationDbContext context;

        public MuscleGroupController(ApplicationDbContext dbContext)
        {
            context = dbContext;
        }

        public IActionResult Index()
        {
            string user = User.Identity.Name;
            ApplicationUser userLoggedIn = context.Users.Single(c => c.UserName == user);
            List<MuscleGroup> musclegroups = context.MuscleGroups.Where(c => c.OwnerId == userLoggedIn.Id).ToList();
            return View(musclegroups);
        }

        public IActionResult Add()
        {
            AddMuscleGroupViewModel addMuscleGroupViewModel = new AddMuscleGroupViewModel();
            return View(addMuscleGroupViewModel);
        }

        [HttpPost]
        public IActionResult Add(AddMuscleGroupViewModel addMuscleGroupViewModel)
        {
            if (ModelState.IsValid)
            {
                string user = User.Identity.Name;
                ApplicationUser userLoggedIn = context.Users.Single(c => c.UserName == user);
                // Add the new cheese to my existing cheeses
                MuscleGroup newMuscleGroup = new MuscleGroup
                {
                    Name = addMuscleGroupViewModel.Name,
                    OwnerId = userLoggedIn.Id
                };

                context.MuscleGroups.Add(newMuscleGroup);
                context.SaveChanges();

                return Redirect("/MuscleGroup");
            }

            return View(addMuscleGroupViewModel);
        }
    }
}
