using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WorkoutGenerator.Models;
using WorkoutGenerator.Data;
using WorkoutGenerator.ViewModels;

namespace WorkoutGenerator.Controllers
{

    public class ExerciseController : Controller
    {
        private ApplicationDbContext context;

        public ExerciseController(ApplicationDbContext dbContext)
        {
            context = dbContext;
        }

        //[Authorize] This attribute will require login before allowing access.  Will be redirected after success.
        //[AllowAnonymous]  This attribute will allow access if global authorization is enabled.
        public IActionResult Index()
        {
            //Filter exercises in View() to only show exercises by user.
           string user = User.Identity.Name;
           ApplicationUser userLoggedIn = context.Users.Single(c => c.UserName == user);
           var filteredExercises = context.Exercises.Where(c => c.OwnerId == userLoggedIn.Id).ToList();
           return View(filteredExercises);
        }

        public IActionResult Add()
        {
            string user = User.Identity.Name;
            ApplicationUser userLoggedIn = context.Users.Single(c => c.UserName == user);
            AddExerciseViewModel addExerciseViewModel = new AddExerciseViewModel(context.MuscleGroups.Where(c => c.OwnerId == userLoggedIn.Id).ToList());
            return View(addExerciseViewModel);
        }

        [HttpPost]
        public IActionResult Add(AddExerciseViewModel addExerciseViewModel)
        {
            string user = User.Identity.Name;
            ApplicationUser userLoggedIn = context.Users.Single(c => c.UserName == user);
            if (ModelState.IsValid)
            {
                //Create user id connection to put into new exercise, linking ApplciationUser and Exercise
                
                // Add the new Exercise to my existing exercises
                MuscleGroup newMuscleGroup =
                    context.MuscleGroups.Single(c => c.MuscleGroupID == addExerciseViewModel.MuscleGroupID);
                DateTime datecreated = DateTime.Now;  //Created outside so if I use for loop in future, all iterations will be the same.
                Exercise newExercise = new Exercise

                {
                    Name = addExerciseViewModel.Name,
                    Description = addExerciseViewModel.Description,
                    MuscleGroup = newMuscleGroup,
                    OwnerId = userLoggedIn.Id,
                    DateCreated = datecreated//Pay attention to this, creates time stamp for creation of entry
                };

                context.Exercises.Add(newExercise);
                context.SaveChanges();

                return Redirect("/Exercise");
            }
            else
            {
                AddExerciseViewModel populateFields = new AddExerciseViewModel(context.MuscleGroups.Where(c => c.OwnerId == userLoggedIn.Id).ToList());
                //This is needed in case the ModelState is not valid, it will keep the categories drop down populated.
                return View(populateFields);
            }
        }

        public IActionResult Remove()
        {
            string user = User.Identity.Name;
            ApplicationUser userLoggedIn = context.Users.Single(c => c.UserName == user);
            ViewBag.title = "Remove Exercises";
            ViewBag.exercises = context.Exercises.Where(c => c.OwnerId == userLoggedIn.Id).ToList();
            return View();
        }

        [HttpPost]
        public IActionResult Remove(int[] exerciseIds)
        {
            foreach (int exerciseId in exerciseIds)
            {
                Exercise theExercise = context.Exercises.Single(c => c.ExerciseID == exerciseId);
                context.Exercises.Remove(theExercise);
            }

            context.SaveChanges();

            return Redirect("/");
        }
    }
}
