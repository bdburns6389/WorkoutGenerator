using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WorkoutGenerator.Data;

namespace WorkoutGenerator.Controllers
{
    public class ExerciseRecordController : Controller
    {
        private ApplicationDbContext context;

        public ExerciseRecordController(ApplicationDbContext dbContext)
        {
            context = dbContext;
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}