using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using RandomWorkout.Data;
using RandomWorkout.Models;
using RandomWorkout.ViewModels;

namespace RandomWorkout.Controllers
{
    public class MuscleGroupController : Controller
    {
        private readonly ExerciseDbContext context;

        public MuscleGroupController(ExerciseDbContext dbContext)
        {
            context = dbContext;
        }

        public IActionResult Index()
        {
            List<MuscleGroup> musclegroups = context.MuscleGroups.ToList();
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
                // Add the new cheese to my existing cheeses
                MuscleGroup newMuscleGroup = new MuscleGroup
                {
                    Name = addMuscleGroupViewModel.Name,
                };

                context.MuscleGroups.Add(newMuscleGroup);
                context.SaveChanges();

                return Redirect("/MuscleGroup");
            }

            return View(addMuscleGroupViewModel);
        }
    }
}
