using System.ComponentModel.DataAnnotations;

namespace RandomWorkout.ViewModels
{
    public class AddMuscleGroupViewModel
    {
        [Required]
        [Display(Name = "Muscle Group Name")]
        public string Name { get; set; }

    }
}
