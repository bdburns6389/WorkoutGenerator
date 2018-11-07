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

namespace WorkoutGenerator.Controllers
{
    public class MuscleGroupController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;

        public MuscleGroupController(ApplicationDbContext dbContext, IConfiguration config)
        {
            _context = dbContext;
            _configuration = config;
        }

        public IActionResult Index()
        {
            var user = User.Identity.Name;
            var userLoggedIn = _context.Users.Single(c => c.UserName == user);
            using (IDbConnection db = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                const string sql = "SELECT * FROM MuscleGroups WHERE OwnerId = @userLoggedIn";
                var muscleGroups = db.Query<MuscleGroup>(sql, new {userLoggedIn = userLoggedIn.Id}).ToList();
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
                string user = User.Identity.Name;
                ApplicationUser userLoggedIn = _context.Users.Single(c => c.UserName == user);

                using (IDbConnection db = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    const string sql = "INSERT INTO MuscleGroups (Name, OwnerId) VALUES (@Name, @OwnerId);";
                    db.Execute(sql, new { addMuscleGroupViewModel.Name, OwnerId = userLoggedIn.Id });
                    return Redirect("/MuscleGroup");
                }
            }

            return View(addMuscleGroupViewModel);
        }

        public IActionResult Remove()
        {
            string user = User.Identity.Name;
            ApplicationUser userLoggedIn = _context.Users.Single(c => c.UserName == user);
            ViewBag.title = "Remove Muscle Groups";

            using (IDbConnection db = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                const string sql = "SELECT * FROM MuscleGroups WHERE OwnerId = @userLoggedIn";
                var muscleGroups = db.Query<MuscleGroup>(sql, new { userLoggedIn = userLoggedIn.Id }).ToList();
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
