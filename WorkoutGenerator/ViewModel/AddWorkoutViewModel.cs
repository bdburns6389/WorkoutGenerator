using System.ComponentModel.DataAnnotations;

namespace RandomWorkout.ViewModels
{
    public class AddWorkoutViewModel
    {
        [Required]
        [Display(Name = "Workout Name")]
        public string Name { get; set; }
    }
}
