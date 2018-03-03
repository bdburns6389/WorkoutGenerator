using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RandomWorkout.Data;
using RandomWorkout.Models;
using RandomWorkout.ViewModels;

namespace RandomWorkout.Controllers
{

    public class ExerciseController : Controller
    {
        private ExerciseDbContext context;

        public ExerciseController(ExerciseDbContext dbContext)
        {
            context = dbContext;
        }

        // GET: /<controller>/
        public IActionResult Index()
        {
            //List<Cheese> cheeses = context.Cheeses.ToList();
            IList<Exercise> exercises = context.Exercises.Include(c => c.MuscleGroup).ToList();

            return View(exercises);
        }

        public IActionResult Add()
        {
            AddExerciseViewModel addExerciseViewModel = new AddExerciseViewModel(context.MuscleGroups.ToList());
            return View(addExerciseViewModel);
        }

        [HttpPost]
        public IActionResult Add(AddExerciseViewModel addCheeseViewModel)
        {

            if (ModelState.IsValid)
            {
                // Add the new cheese to my existing cheeses
                MuscleGroup newMuscleGroup =
                    context.MuscleGroups.Single(c => c.ID == addCheeseViewModel.MuscleGroupID);
                Exercise newExercise = new Exercise

                {
                    Name = addCheeseViewModel.Name,
                    Description = addCheeseViewModel.Description,
                    MuscleGroup = newMuscleGroup
                };

                context.Exercises.Add(newExercise);
                context.SaveChanges();

                return Redirect("/Exercise");
            }
            else
            {
                AddExerciseViewModel populateFields = new AddExerciseViewModel(context.MuscleGroups.ToList());
                //This is needed in case the ModelState is not valid, it will keep the categories drop down populated.
                return View(populateFields);
            }
        }

        public IActionResult Remove()
        {
            ViewBag.title = "Remove Exercises";
            ViewBag.exercises = context.Exercises.ToList();
            return View();
        }

        [HttpPost]
        public IActionResult Remove(int[] exerciseIds)
        {
            foreach (int exerciseId in exerciseIds)
            {
                Exercise theExercise = context.Exercises.Single(c => c.ID == exerciseId);
                context.Exercises.Remove(theExercise);
            }

            context.SaveChanges();

            return Redirect("/");
        }
    }
}
