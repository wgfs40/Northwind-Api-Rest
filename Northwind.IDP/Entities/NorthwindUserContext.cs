using Microsoft.EntityFrameworkCore;

namespace Marvin.IDP.Entities
{
    public class NorthwindUserContext : DbContext
    {
        public NorthwindUserContext(DbContextOptions<NorthwindUserContext> options)
           : base(options)
        {
           
        }

        public DbSet<User> Users { get; set; }
    }
}
