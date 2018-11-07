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
        private readonly IConfiguration _configuration;
        private readonly UserManager<ApplicationUser> _userManager;

        public ExerciseController(IConfiguration config, UserManager<ApplicationUser> userManager)
        {
            _configuration = config;
            _userManager = userManager;
        }

        public IActionResult Index()
        {
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
            var userId = _userManager.GetUserId(User);
            using (IDbConnection db = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                const string sql = "SELECT * FROM MuscleGroups WHERE OwnerId = @userId";
                var newExerciseViewModel = db.Query<MuscleGroup>(sql, new { userId });
                var addExerciseViewModel = new AddExerciseViewModel(newExerciseViewModel);
                return View(addExerciseViewModel);

            }
        }

        [HttpPost]
        public IActionResult Add(AddExerciseViewModel addExerciseViewModel)
        {
            using (IDbConnection db = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                var userId = _userManager.GetUserId(User);
                if (ModelState.IsValid)
                {
                    const string sql = "SELECT * FROM MuscleGroups WHERE MuscleGroupID = @MuscleGroupID";

                    var newMuscleGroup =
                        db.Query<MuscleGroup>(sql, new { addExerciseViewModel.MuscleGroupID })
                            .FirstOrDefault();

                    var dateCreated = DateTime.Now;

                    const string exerciseSql =
                        "INSERT INTO Exercises (Name, Description, MuscleGroupID, OwnerID, DateCreated) " +
                        "VALUES (@Name, @Description, @MuscleGroupID, @OwnerID, @DateCreated);";

                    db.Execute(exerciseSql, new
                    {
                        addExerciseViewModel.Name,
                        addExerciseViewModel.Description,
                        newMuscleGroup.MuscleGroupID,
                        OwnerID = userId,
                        DateCreated = dateCreated
                    });
                    return Redirect("/Exercise");
                }

                const string sqlElse = "SELECT * FROM MuscleGroups WHERE OwnerId = @userId";
                var muscleGroupElse = db.Query<MuscleGroup>(sqlElse, new {userId}).ToList();
                //This is needed in case the ModelState is not valid, it will keep the categories drop down populated.   
                var populateFields = new AddExerciseViewModel(muscleGroupElse);
                return View(populateFields);
            }
         
        }

        public IActionResult Remove()
        {
            var userId = _userManager.GetUserId(User);
            ViewBag.title = "Remove Exercises";
            using (IDbConnection db = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                const string sql = "SELECT * FROM Exercises WHERE OwnerId = @userLoggedIn";
                var exercises = db.Query<Exercise>(sql, new { userLoggedIn = userId }).ToList();
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
