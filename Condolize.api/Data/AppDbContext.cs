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

    }
}
