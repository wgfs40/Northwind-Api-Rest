
namespace Web_Identity
{
    using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore;

    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            :base(options)
        {
        }

        //public  DbSet<ApplicationUser> Users { get; set; }
        //public  DbSet<UserClaim> UserClaims { get; set; }
        //public  DbSet<ApplicationRole> Roles { get; set; }
        //public  DbSet<ApplicationUserRole> UserRoles { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder); // This needs to go before the other rules!

            //modelBuilder.Entity<IdentityUserClaim<string>>()                      
            //    .ToTable("UserClaims");

            //modelBuilder.Entity<ApplicationUser>()
            //    .ToTable("Users");

            //modelBuilder.Entity<ApplicationRole>()
            //    .ToTable("Roles");

            //modelBuilder.Entity<ApplicationUserRole>().HasKey(r => new { UserId = r.UserId, RoleId = r.RoleId });
            //modelBuilder.Entity<ApplicationUser>().HasMany<ApplicationUserRole>((ApplicationUser u) => u.UserRoles);


            //modelBuilder.Entity<ApplicationUserRole>().ToTable("UserRoles");
        }

    }
}