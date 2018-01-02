using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Web_App.ViewModels;

namespace Web_App
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser,ApplicationRole,string>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            :base(options)
        {
        }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
           base.OnModelCreating(modelBuilder);


            //modelBuilder.Entity<ApplicationUserRole>(option =>
            //{
            //    option.HasKey(ur => new { ur.UserId, ur.RoleId });
            //    option.HasIndex(ur => ur.RoleId);

            //    option.Property(u => u.UserId).HasMaxLength(450);
            //    option.Property(u => u.RoleId).HasMaxLength(450);

            //    option.HasOne(r => r.Roles)
            //        .WithMany()
            //        .HasForeignKey(r => r.RoleId)
            //        .OnDelete(DeleteBehavior.Cascade);

            //    option.HasOne(u => u.Users)
            //        .WithMany()
            //        .HasForeignKey(u => u.UserId)
            //        .OnDelete(DeleteBehavior.Cascade);

            //    option.ToTable("UserRole");
            //});

            ////Users
            //modelBuilder.Entity<ApplicationUser>(useroption =>
            //{
            //    useroption.HasKey(u => u.Id);
            //    useroption.Property(u => u.Id).HasMaxLength(450);
               
            //    useroption.HasIndex(u => u.NormalizedEmail).HasName("EmailIndex");
            //    useroption.ToTable("Users");
            //});

            ////Roles
            //modelBuilder.Entity<ApplicationRole>(roleoption =>
            //{
            //    roleoption.HasKey(r => r.Id);
            //    roleoption.Property(r => r.Id).HasMaxLength(450);
            //    roleoption.Property<string>(r => r.Id).ValueGeneratedOnAdd();
            //    roleoption.ToTable("Roles");
            //});

            //UserClaim
            //modelBuilder.Entity<ApplicationUserClaim>(userclaimopt =>
            //{
            //    userclaimopt.HasKey(u => u.Id);
            //    userclaimopt.Property(u => u.Id).HasColumnType("nvarchar").HasMaxLength(450);
            //    userclaimopt.Property(u => u.Id).ValueGeneratedOnAdd();
            //    userclaimopt.Property(u => u.ClaimType).HasColumnType("nvarchar").HasMaxLength(500);
            //    userclaimopt.Property(u => u.ClaimValue).HasColumnType("nvarchar").HasMaxLength(500);
            //    userclaimopt.Property(u => u.UserId).HasColumnType("nvarchar").HasMaxLength(500).IsRequired();
            //    userclaimopt.HasIndex(u => u.UserId);

            //    userclaimopt.HasOne(c => c.Users)
            //        .WithMany()
            //        .HasForeignKey(c => c.UserId)
            //        .OnDelete(DeleteBehavior.Cascade);

            //    userclaimopt.HasKey(c => c.Id);
            //    userclaimopt.HasIndex(c => c.UserId);
            //    userclaimopt.HasOne(c => c.Users)
            //        .WithMany()
            //        .HasForeignKey(c => c.UserId)
            //        .OnDelete(DeleteBehavior.Cascade);

            //    userclaimopt.ToTable("UserClaims");
            //});

            //RoleClaim
            //modelBuilder.Entity<ApplicationRoleClaim>(roleclaimoption =>
            //{
            //    roleclaimoption.HasKey(r => r.Id);
            //    roleclaimoption.Property(r => r.Id).HasColumnType("nvarchar").HasMaxLength(450);

            //    roleclaimoption.Property(r => r.Id).ValueGeneratedOnAdd();
            //    roleclaimoption.Property(r => r.ClaimType).HasColumnType("nvarchar").HasMaxLength(500);
            //    roleclaimoption.Property(r => r.ClaimValue).HasColumnType("nvarchar").HasMaxLength(500);
            //    roleclaimoption.Property(r => r.RoleId).HasColumnType("nvarchar").HasMaxLength(500).IsRequired();
            //    roleclaimoption.HasIndex(r => r.RoleId);

            //    roleclaimoption.HasOne(r => r.Roles)
            //        .WithMany()
            //        .HasForeignKey(r => r.RoleId)
            //        .OnDelete(DeleteBehavior.Cascade);

            //    roleclaimoption.HasKey(r => r.Id);
            //    roleclaimoption.HasIndex(r => r.RoleId);
            //    roleclaimoption.HasOne(r => r.Roles)
            //        .WithMany()
            //        .HasForeignKey(r => r.RoleId)
            //        .OnDelete(DeleteBehavior.Cascade);

            //    roleclaimoption.ToTable("RoleClaims");
            //});
        }
    }
}
