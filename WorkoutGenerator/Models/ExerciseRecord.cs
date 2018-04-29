using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WorkoutGenerator.Models
{
    public class ExerciseRecord
    {
        public Guid ExerciseID { get; set; }
        public Exercise Exercise { get; set; }

        public Guid RecordID { get; set; }
        public Record Record { get; set; }
    }
}