using Microsoft.EntityFrameworkCore;


namespace WorkoutGenerator.Models
{
    public class RDSContext : DbContext
    {
        public RDSContext()
          : base(Helpers.GetRDSConnectionString())
        {
        }

        public static RDSContext Create()
        {
            return new RDSContext();
        }
    }
}