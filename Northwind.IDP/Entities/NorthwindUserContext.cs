using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Northwind.IDP.Entities
{
    public class NorthwindUserContext : IdentityDbContext<ApplicationUser, ApplicationRole,string, ApplicationUserClaim,ApplicationUserRole, 
        ApplicationUserLogin, ApplicationRoleClaim, ApplicationUserToken>
    {
        public NorthwindUserContext(DbContextOptions<NorthwindUserContext> options)
           : base(options)
        {
           
        }

        public new DbSet<User> Users { get; set; }        
        //public DbSet<ApplicationUser> ApplicationUsers { get; set; }
        //public DbSet<ApplicationUserClaim> UserClaims { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {   
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<ApplicationUser>(option => {
                option.HasKey(u => u.Id);
                option.Property(u => u.Id).HasMaxLength(450);
                option.ToTable("ApplicationUsers");
            });

            modelBuilder.Entity<ApplicationRole>(option => {
                option.HasKey(r => r.Id);
                option.ToTable("ApplicationRoles");
            });

            modelBuilder.Entity<ApplicationUserClaim>(option => {
                option.HasKey(uc => uc.Id);
                option.Property(uc => uc.UserId).HasMaxLength(450);
                option.ToTable("ApplicationUserClaim");
            });

            modelBuilder.Entity<ApplicationUserRole>(option => {
                option.HasKey(ur => new { ur.RoleId, ur.UserId });
                option.ToTable("ApplicationUserRole");
            });

            modelBuilder.Entity<ApplicationUserLogin>(option => {
                option.HasKey(ul => ul.UserId);
                option.ToTable("ApplicationUserLogin");
            });

            modelBuilder.Entity<ApplicationRoleClaim>(option => {
                option.HasKey(rc => rc.Id);
                option.ToTable("ApplicationRoleClaim");
            });

            modelBuilder.Entity<ApplicationUserToken>(option => {
                option.HasKey(ut => ut.UserId);
                option.ToTable("ApplicationUserToken");
            });

            //modelBuilder.Entity<ApplicationUserClaim>(option => {
            //    option.HasKey(u => u.Id);
            //    option.Property(u => u.UserId).HasMaxLength(450);

            //    option.HasOne<ApplicationUser>()
            //        .WithMany(c => c.UserClaims)
            //        .HasForeignKey(u => u.UserId)
            //        .OnDelete(DeleteBehavior.Cascade);

            //    option.ToTable("UserClaimsIdentity");
            //});



        }
    }
}
