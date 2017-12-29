using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Web_App.ViewModels;

namespace Web_App
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            :base(options)
        {
        }

        public DbSet<ApplicationUser> Users { get; set; }
        public DbSet<ApplicationRole> Roles { get; set; }
        public DbSet<ApplicationUserClaim> UserClaims { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //Users
            modelBuilder.Entity<ApplicationUser>(useroption =>
            {
                useroption.HasKey(u => u.Id);
                useroption.Property<string>(u => u.Id).ValueGeneratedOnAdd();
                useroption.HasIndex(u => u.NormalizedEmail).HasName("EmailIndex");
                useroption.ToTable("Users");
            });

            //Roles
            modelBuilder.Entity<ApplicationRole>(roleoption => {
                roleoption.HasKey(r => r.Id);
                roleoption.Property<string>(r => r.Id).ValueGeneratedOnAdd();
                roleoption.ToTable("Roles");
            });

            //UserClaim
            modelBuilder.Entity<ApplicationUserClaim>(userclaimopt => {
                userclaimopt.HasKey(c => c.Id);
                userclaimopt.HasIndex(c => c.UserId);
                userclaimopt.HasOne(c => c.Users)
                    .WithMany()
                    .HasForeignKey(c => c.UserId)
                    .OnDelete(DeleteBehavior.Cascade);
                
                userclaimopt.ToTable("UserClaims");
            });

            //RoleClaim
            modelBuilder.Entity<ApplicationRoleClaim>(roleclaimoption => {
                roleclaimoption.HasKey(r => r.Id);
                roleclaimoption.HasIndex(r => r.RoleId);
                roleclaimoption.HasOne(r => r.Roles)
                    .WithMany()
                    .HasForeignKey(r => r.RoleId)
                    .OnDelete(DeleteBehavior.Cascade);

                roleclaimoption.ToTable("RoleClaims");
            });
        }
    }
}
