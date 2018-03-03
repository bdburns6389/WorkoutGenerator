using System.ComponentModel.DataAnnotations;

namespace RandomWorkout.ViewModels
{
    public class AddWorkoutViewModel
    {
        [Required]
        [Display(Name = "Menu Name")]
        public string Name { get; set; }
    }
}
