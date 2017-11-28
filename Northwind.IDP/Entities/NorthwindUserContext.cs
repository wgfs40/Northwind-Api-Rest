using Microsoft.EntityFrameworkCore;

namespace Northwind.IDP.Entities
{
    public class NorthwindUserContext : DbContext
    {
        public NorthwindUserContext(DbContextOptions<NorthwindUserContext> options)
           : base(options)
        {
           
        }

        public DbSet<User> Users { get; set; }
        public DbSet<ApplicationUser> ApplicationUsers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {          
            
            base.OnModelCreating(modelBuilder);
        }
    }
}
