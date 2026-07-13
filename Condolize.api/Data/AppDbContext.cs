using Condolize.api.Entities;
using Microsoft.EntityFrameworkCore;

namespace Condolize.api.Data
{
    public class AppDbContext : DbContext
    {

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Association> Associations => Set<Association>();
        public DbSet<User> Users => Set<User>();
        public DbSet<Unit> Units => Set<Unit>();
        public DbSet<Resident> Residents => Set<Resident>();
        public DbSet<PublicSpace> PublicSpaces => Set<PublicSpace>();

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<PublicSpace>()
                .Property(x => x.Amenities)
                .HasColumnType("jsonb");
        }

    }
}
