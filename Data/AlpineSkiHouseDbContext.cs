using Microsoft.EntityFrameworkCore;
using System.Diagnostics;



namespace AlpineSkiHouse
{
    public class AlpineSkiHouseDbContext : DbContext
    {
        public AlpineSkiHouseDbContext(DbContextOptions<AlpineSkiHouseDbContext> options)
            : base(options)
        {
        }

        public DbSet<Activity> Activities { get; set; }
    }
}
