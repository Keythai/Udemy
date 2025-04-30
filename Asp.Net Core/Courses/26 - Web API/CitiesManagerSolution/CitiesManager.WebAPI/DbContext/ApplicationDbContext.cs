using CitiesManager.WebAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace CitiesManager.WebAPI.DatabaseContext
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }
        public ApplicationDbContext() { }
        public virtual DbSet<City> Cities { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<City>()
                .HasData(new City() { CityId = Guid.Parse("7A3B81DA-3355-46EB-9A3E-B009555053F9"), CityName="New York" });
            modelBuilder.Entity<City>()
                .HasData(new City() { CityId = Guid.Parse("8EB50443-A3D0-4BEE-9599-FA41DC912DCA"), CityName = "London" });
        }
    }
}
