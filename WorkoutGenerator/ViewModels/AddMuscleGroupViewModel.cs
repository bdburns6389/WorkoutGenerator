using System.ComponentModel.DataAnnotations;

namespace WorkoutGenerator.ViewModels
{
    public class AddMuscleGroupViewModel
    {
        [Required]
        [Display(Name = "Muscle Group Name")]
        public string Name { get; set; }
        public string OwnerId { get; set; }

        public AddMuscleGroupViewModel() { }

    }
}
