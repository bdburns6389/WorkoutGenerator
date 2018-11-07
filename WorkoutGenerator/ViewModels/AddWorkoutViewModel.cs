using System.ComponentModel.DataAnnotations;

namespace WorkoutGenerator.ViewModels
{
    public class AddWorkoutViewModel
    {
        [Required]
        [Display(Name = "Workout Name")]
        public string Name { get; set; }
        public string OwnerId { get; set; }

        public AddWorkoutViewModel() { }
    }
}
