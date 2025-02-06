using Microsoft.EntityFrameworkCore;
using AlpineSkiHouse.Models;

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
