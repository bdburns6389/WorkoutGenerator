using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using WorkoutGenerator.Data;
using WorkoutGenerator.Models;
using WorkoutGenerator.ViewModels;
using Dapper;
using Microsoft.AspNetCore.Identity;

namespace WorkoutGenerator.Controllers
{
    public class MuscleGroupController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly UserManager<ApplicationUser> _userManager;

        public MuscleGroupController(IConfiguration config, UserManager<ApplicationUser> userManger)
        {
            _configuration = config;
            _userManager = userManger;
        }

        public IActionResult Index()
        {
            var userId = _userManager.GetUserId(User);
            using (IDbConnection db = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                const string sql = "SELECT * FROM MuscleGroups WHERE OwnerId = @userLoggedIn";
                var muscleGroups = db.Query<MuscleGroup>(sql, new {userLoggedIn = userId}).ToList();
                return View(muscleGroups);
            }
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
                var userId = _userManager.GetUserId(User);

                using (IDbConnection db = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    const string sql = "INSERT INTO MuscleGroups (Name, OwnerId) VALUES (@Name, @OwnerId);";
                    db.Execute(sql, new { addMuscleGroupViewModel.Name, OwnerId = userId });
                    return Redirect("/MuscleGroup");
                }
            }

            return View(addMuscleGroupViewModel);
        }

        public IActionResult Remove()
        {
            var userId = _userManager.GetUserId(User);
            ViewBag.title = "Remove Muscle Groups";

            using (IDbConnection db = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                const string sql = "SELECT * FROM MuscleGroups WHERE OwnerId = @userLoggedIn";
                var muscleGroups = db.Query<MuscleGroup>(sql, new { userLoggedIn = userId }).ToList();
                ViewBag.musclegroups = muscleGroups;
            }

            return View();
        }

        [HttpPost]
        public IActionResult Remove(int[] muscleGroupIds)
        {
            foreach (var muscleGroupId in muscleGroupIds)
            {
                using (IDbConnection db = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    const string sql = "DELETE FROM MuscleGroups WHERE MuscleGroupID = @muscleGroupId";
                    db.Execute(sql, new { muscleGroupId });
                }
            }

            return Redirect("/");
        }
    }
}
