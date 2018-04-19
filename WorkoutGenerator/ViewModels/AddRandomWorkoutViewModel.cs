using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WorkoutGenerator.ViewModels
{
    public class AddRandomWorkoutViewModel
    {
        [Display(Name = "Choose a Name for your workout")]
        public string Name { get; set; }

        public AddRandomWorkoutViewModel()
        {

        }
    }
}
