

namespace Web_Identity
{
    using Microsoft.EntityFrameworkCore;
    using Web_Identity.Models;

    public class NorthwindContext : DbContext
    {
        public NorthwindContext(DbContextOptions<NorthwindContext> options):base(options)
        {

        }

        DbSet<Customer> Customers { get; set; }
        DbSet<Order> Orders { get; set; }
        DbSet<OrderDetail> OrderDetails { get; set; }
        
    }
}
