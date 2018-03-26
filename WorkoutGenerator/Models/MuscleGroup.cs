using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WorkoutGenerator.Models;

namespace WorkoutGenerator.Models
{
    public class MuscleGroup
    {
        [Key]
        public int MuscleGroupID { get; set; }
        public string Name { get; set; }
        

        //OwnerId and ApplicationUser User is used to make a OneToMany Relationship 
        [ForeignKey("Id")]
        public string OwnerId { get; set; }
        public virtual ApplicationUser User { get; set; }

        public IList<Exercise> Exercises { get; set; }
    }
}
