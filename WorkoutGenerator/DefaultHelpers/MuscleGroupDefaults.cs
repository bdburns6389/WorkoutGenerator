using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WorkoutGenerator.Data;
using WorkoutGenerator.Models;

namespace WorkoutGenerator.DefaultHelpers
{
    public class MuscleGroupDefaults
    {
        public static List<string> defaults = new List<string>
        {
            "Legs", "Back", "Chest", "Arms", "Abdominals"
        };

       

        public static void Add(string id, ApplicationDbContext context)
        {


            foreach (string i in defaults)
            {
                MuscleGroup newMuscleGroup = new MuscleGroup
                {
                    Name = i,
                    OwnerId = id
                };
                context.Add(newMuscleGroup);
                context.SaveChanges();
            }
        }
    }
}
