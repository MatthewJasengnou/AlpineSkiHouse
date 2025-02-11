using Microsoft.EntityFrameworkCore;
using AlpineSkiHouse.Models;

namespace AlpineSkiHouse
{
    public class AlpineSkiHouseDbContext : DbContext
    {
        public DbSet<Activity> Activities { get; set; }

        public AlpineSkiHouseDbContext(DbContextOptions<AlpineSkiHouseDbContext> options) : base(options)
        {
        }

        // Configure your model relationships and database schema here
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Example: configuring a default length for strings if you haven't set one globally
            modelBuilder.Entity<Activity>()
                .Property(a => a.Name)
                .IsRequired()
                .HasMaxLength(100);
        }
    }
}
