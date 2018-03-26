using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using WorkoutGenerator.Models;

namespace WorkoutGenerator.ViewModels
{
    public class AddExerciseViewModel
    {
        [Required]
        [Display(Name = "Exercise Name")]
        public string Name { get; set; }

        [Required(ErrorMessage = "You must give your exercise a description")]
        public string Description { get; set; }

        [Required]
        [Display(Name = "Muscle Group")]
        public int MuscleGroupID { get; set; }
        public List<SelectListItem> MuscleGroups { get; set; }
        //should refer to key for linking to Application user.
        public string OwnerId { get; set; }
        public DateTime DateCreated { get; set; }
        public AddExerciseViewModel()
        {

        }

        public AddExerciseViewModel(IEnumerable<MuscleGroup> muscleGroups)
        {

            MuscleGroups = new List<SelectListItem>();

            // <option value="0">Hard</option>i
            foreach (MuscleGroup i in muscleGroups)
            {
                MuscleGroups.Add(new SelectListItem
                {
                    Value = i.MuscleGroupID.ToString(),
                    Text = i.Name
                });
            }
        }
    }
}