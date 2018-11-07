using System;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Protocols;
using WorkoutGenerator.Models;
using WorkoutGenerator.Data;
using WorkoutGenerator.ViewModels;
using Dapper;
using System.Configuration;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;

namespace WorkoutGenerator.Controllers
{

    public class ExerciseController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;
        private readonly UserManager<ApplicationUser> _userManager;

        public ExerciseController(ApplicationDbContext dbContext, IConfiguration config, UserManager<ApplicationUser> userManager)
        {
            _configuration = config;
            _context = dbContext;
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            //TODO Change all occurences to this instead, using userManager.
            var userId = _userManager.GetUserId(User);
            using (IDbConnection db = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                const string sql = "SELECT * FROM dbo.[Exercises] WHERE OwnerId = @UserLoggedIn";
                var filteredExercises = db.Query<Exercise>(sql, new { UserLoggedIn = userId }).ToList();
                return View(filteredExercises);
            }
        }

        public IActionResult Add()
        {
            var user = User.Identity.Name;
            var userLoggedIn = _context.Users.Single(c => c.UserName == user);
            var addExerciseViewModel = new AddExerciseViewModel(_context.MuscleGroups.Where(c => c.OwnerId == userLoggedIn.Id).ToList());
            return View(addExerciseViewModel);
        }

        [HttpPost]
        public IActionResult Add(AddExerciseViewModel addExerciseViewModel)
        {
            string user = User.Identity.Name;
            ApplicationUser userLoggedIn = _context.Users.Single(c => c.UserName == user);
            if (ModelState.IsValid)
            {
                // Add the new Exercise to my existing exercises
                var newMuscleGroup =
                   _context.MuscleGroups.Single(c => c.MuscleGroupID == addExerciseViewModel.MuscleGroupID);
                var dateCreated = DateTime.Now;

                const string exerciseSql = "INSERT INTO Exercises (Name, Description, MuscleGroupID, OwnerID, DateCreated) " +
                                           "VALUES (@Name, @Description, @MuscleGroupID, @OwnerID, @DateCreated);";
                using (IDbConnection db = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    db.Execute(exerciseSql, new
                    {
                        addExerciseViewModel.Name,
                        addExerciseViewModel.Description,
                        newMuscleGroup.MuscleGroupID,
                        OwnerID = userLoggedIn.Id,
                        DateCreated = dateCreated
                    });
                }
                return Redirect("/Exercise");
            }
            else
            {
                var populateFields = new AddExerciseViewModel(_context.MuscleGroups.Where(c => c.OwnerId == userLoggedIn.Id).ToList());
                //This is needed in case the ModelState is not valid, it will keep the categories drop down populated.
                return View(populateFields);
            }
        }

        public IActionResult Remove()
        {
            var user = User.Identity.Name;
            var userLoggedIn = _context.Users.Single(c => c.UserName == user);
            ViewBag.title = "Remove Exercises";
            using (IDbConnection db = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                const string sql = "SELECT * FROM Exercises WHERE OwnerId = @userLoggedIn";
                var exercises = db.Query<Exercise>(sql, new {userLoggedIn = userLoggedIn.Id}).ToList();
                ViewBag.exercises = exercises;
                return View();
            }
        }

        [HttpPost]
        public IActionResult Remove(int[] exerciseIds)
        {
            foreach (var exerciseId in exerciseIds)
            {
                using (IDbConnection db = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    const string sql = "DELETE FROM Exercises WHERE ExerciseID = @exerciseId";
                    db.Execute(sql, new { exerciseId });
                }
            }
            return Redirect("/");
        }
    }
}
