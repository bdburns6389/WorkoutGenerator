using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RandomWorkout.Models;
using RandomWorkout.ViewModels;
using WorkoutGenerator.Data;
using WorkoutGenerator.Models;

namespace RandomWorkout.Controllers
{

    public class ExerciseController : Controller
    {
        private ApplicationDbContext context;

        public ExerciseController(ApplicationDbContext dbContext)
        {
            context = dbContext;
        }

        // GET: /<controller>/
        //[Authorize] This attribute will require login before allowing access.  Will be redirected after success.
        //[AllowAnonymous]  This attribute will allow access if global authorization is enabled.
        public IActionResult Index()
        {
            //List<Cheese> cheeses = context.Cheeses.ToList();

            //TODO #1 Try to filter exercises in View() to only show exercises by user.
            string user = User.Identity.Name;
            ApplicationUser userLoggedIn = context.Users.Single(c => c.UserName == user);


            IList<Exercise> exercises = context.Exercises.Include(c => c.MuscleGroup).ToList();
            
            return View(exercises);
        }

        public IActionResult Add()
        {
            AddExerciseViewModel addExerciseViewModel = new AddExerciseViewModel(context.MuscleGroups.ToList());
            return View(addExerciseViewModel);
        }

        [HttpPost]
        public IActionResult Add(AddExerciseViewModel addExerciseViewModel)
        {

            if (ModelState.IsValid)
            {
                //Create user id connection to put into new exercise, linking ApplciationUser and Exercise
                string user = User.Identity.Name;
                ApplicationUser userLoggedIn = context.Users.Single(c => c.UserName == user);
                // Add the new Exercise to my existing exercises
                MuscleGroup newMuscleGroup =
                    context.MuscleGroups.Single(c => c.ID == addExerciseViewModel.MuscleGroupID);
                Exercise newExercise = new Exercise

                {
                    Name = addExerciseViewModel.Name,
                    Description = addExerciseViewModel.Description,
                    MuscleGroup = newMuscleGroup,
                    OwnerId = userLoggedIn.Id
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
