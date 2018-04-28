using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WorkoutGenerator.Data;
using WorkoutGenerator.Models;
using WorkoutGenerator.ViewModels;

namespace WorkoutGenerator.Controllers
{
    public class WorkoutController : Controller
    {
        private readonly ApplicationDbContext context;

        public WorkoutController(ApplicationDbContext dbContext)
        {
            context = dbContext;
        }

        //[AllowAnonymous]  THis will allow access without logging in while Global authorization is enabled
        //                  in Startup.cs .MVC()
        public IActionResult Index()
        {
            string user = User.Identity.Name;
            ApplicationUser userLoggedIn = context.Users.Single(c => c.UserName == user);
            var filteredWorkouts = context.Workouts.Where(c => c.OwnerId == userLoggedIn.Id).ToList();
            List<Workout> workouts = context.Workouts.ToList();
            return View(filteredWorkouts);
        }
        //[Authorize]  This attribute will redirect to login page to allow access.
        public IActionResult Add()
        {
            AddWorkoutViewModel addMenuViewModel = new AddWorkoutViewModel();
            return View(addMenuViewModel);
        }

        [HttpPost]
        public IActionResult Add(AddWorkoutViewModel addWorkoutViewModel)
        {
            if (ModelState.IsValid)
            {
                //Create user id connection to put into new exercise, linking ApplciationUser and Workout
                string user = User.Identity.Name;
                ApplicationUser userLoggedIn = context.Users.Single(c => c.UserName == user);

                Workout newWorkout = new Workout
                {
                    Name = addWorkoutViewModel.Name,
                    DateCreated = DateTime.Now,
                    OwnerId = userLoggedIn.Id
                };

                context.Workouts.Add(newWorkout);
                context.SaveChanges();

                return Redirect("/Workout/ViewWorkout/" + newWorkout.WorkoutID);
            }

            return View(addWorkoutViewModel);
        }

        public IActionResult ViewWorkout(int id)
        {
            string user = User.Identity.Name;
            ApplicationUser userLoggedIn = context.Users.Single(c => c.UserName == user);
            List<ExerciseWorkout> exercises = context
                .ExerciseWorkouts
                .Include(item => item.Exercise)
                .Where(cm => cm.WorkoutID == id && cm.Workout.OwnerId == userLoggedIn.Id)//cm.Workout.OwnerId == userLoggedIn.Id returns list of owner specific workouts
                .ToList();

            Workout workout = context.Workouts.Single(m => m.WorkoutID == id); //Only needed for title in page and to link to add an exercise

            ViewWorkoutViewModel viewModel = new ViewWorkoutViewModel
            {
                Workout = workout,
                Exercises = exercises
            };

            return View(viewModel);
        }

        public IActionResult AddExercise(int id)
        {
            string user = User.Identity.Name;
            ApplicationUser userLoggedIn = context.Users.Single(c => c.UserName == user);

            Workout workout = context.Workouts.Single(m => m.WorkoutID == id);
            List<Exercise> exercises = context.Exercises.Where(c => c.OwnerId == userLoggedIn.Id).ToList();//OwnerId specifies user for exercise list.

            return View(new AddWorkoutExerciseViewModel(workout, exercises));
        }

        [HttpPost]
        public IActionResult AddExercise(AddWorkoutExerciseViewModel addWorkoutExerciseViewModel)
        {

            if (ModelState.IsValid)
            {
                var exerciseID = addWorkoutExerciseViewModel.ExerciseID;
                var workoutID = addWorkoutExerciseViewModel.WorkoutID;

                IList<ExerciseWorkout> existingItems = context.ExerciseWorkouts
                    .Where(cm => cm.ExerciseID == exerciseID)
                    .Where(cm => cm.WorkoutID == workoutID).ToList();

                if (existingItems.Count == 0)
                {
                    ExerciseWorkout workoutItem = new ExerciseWorkout
                    {
                        Exercise = context.Exercises.Single(e => e.ExerciseID == exerciseID),
                        Workout = context.Workouts.Single(w => w.WorkoutID == workoutID)
                    };

                    context.ExerciseWorkouts.Add(workoutItem);
                    context.SaveChanges();
                }
                return Redirect(string.Format("/Workout/ViewWorkout/{0}", addWorkoutExerciseViewModel.WorkoutID));
            } 
            return View(addWorkoutExerciseViewModel);
        }

        public IActionResult Remove()
        {
            string user = User.Identity.Name;
            ApplicationUser userLoggedIn = context.Users.Single(c => c.UserName == user);
            ViewBag.title = "Remove Workouts";
            ViewBag.workouts = context.Workouts.Where(c => c.OwnerId == userLoggedIn.Id).ToList();
            return View();
        }

        [HttpPost]
        public IActionResult Remove(int [] workoutIds)
        {
            foreach (int workoutId in workoutIds)
            {
                Workout theWorkout = context.Workouts.Single(c => c.WorkoutID == workoutId);
                context.Workouts.Remove(theWorkout);
            }

            context.SaveChanges();

            return Redirect("/");
        }

        public IActionResult Random()
        {

            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        public IActionResult Random(AddRandomWorkoutViewModel addRandomWorkoutViewModel)
        {
            string user = User.Identity.Name;
            ApplicationUser userLoggedIn = context.Users.Single(c => c.UserName == user);


            Workout newWorkout = new Workout
            {
                Name = addRandomWorkoutViewModel.Name,
                DateCreated = DateTime.Now,
                OwnerId = userLoggedIn.Id
            };

            context.Workouts.Add(newWorkout);
            context.SaveChanges();

            int newWorkoutid = newWorkout.WorkoutID;


            List<MuscleGroup> muscleGroups = context.MuscleGroups.Where(c => c.OwnerId == userLoggedIn.Id).ToList();
            //List<MuscleGroup> muscles = new List<MuscleGroup>();// Currently pulls first musclegroup in table
            List<Exercise> empty = new List<Exercise>();// Will Be replaced with Workout model
            foreach(var muscleGroup in muscleGroups)
            {
                Random random = new Random();
                List<Exercise> exercises = context
                    .Exercises
                    .Where(c => c.MuscleGroupID == muscleGroup.MuscleGroupID)
                    .OrderBy(x => (random.Next()))
                    .ToList();
                var single = exercises.FirstOrDefault();

                ExerciseWorkout workoutItem = new ExerciseWorkout
                {
                    Exercise = exercises.FirstOrDefault(),
                    Workout = context.Workouts.Single(w => w.WorkoutID == newWorkoutid)
                };

                context.ExerciseWorkouts.Add(workoutItem);
                

                //Returns a random exercise from given MuscleGroup
                 // TODO add single exercise to newWorkout
            }
            context.SaveChanges();

            return View();
        }
    }
}