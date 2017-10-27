using Microsoft.EntityFrameworkCore;

namespace Northwind.Api.Entities
{
    public class NorthwindContext : DbContext
    {
        public NorthwindContext(DbContextOptions<NorthwindContext> options):
            base(options)
        {
           Database.Migrate();
        }

        public DbSet<Customer> Customers { get; set; }
        public DbSet<Order> Orders { get; set; }
    }
}
