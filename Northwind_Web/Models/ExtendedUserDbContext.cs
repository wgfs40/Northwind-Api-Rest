namespace Northwind_Web.Models
{
    using System.Data.Entity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using Northwind_Web.ViewModels;
    using System.Data.Entity.Infrastructure.Annotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class ExtendedUserDbContext : IdentityDbContext<ExtendedUser>
    {
        public ExtendedUserDbContext(string connectionString): base(connectionString)
        {

        }

        public DbSet<Address>Addresses { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            var address = modelBuilder.Entity<Address>();
            address.ToTable("AspNetUserAddresses");
            address.HasKey(x => x.Id);

            var user = modelBuilder.Entity<ExtendedUser>();
            user.Property(x => x.FullName).IsRequired().HasMaxLength(256)
                .HasColumnAnnotation("Index", new IndexAnnotation(
                    new IndexAttribute("FullNameIndex")));

            user.HasMany(x => x.Addresses).WithRequired().HasForeignKey(x => x.UserId);
        }
    }
}