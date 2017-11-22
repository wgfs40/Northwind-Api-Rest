using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace Northwind.IDP.Entities
{
    public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<NorthwindUserContext>
    {
        NorthwindUserContext IDesignTimeDbContextFactory<NorthwindUserContext>.CreateDbContext(string[] args)
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
               .SetBasePath(Directory.GetCurrentDirectory())
               .AddJsonFile("appsettings.json")
               .Build();
            var builder = new DbContextOptionsBuilder<NorthwindUserContext>();
            var connectionString = configuration.GetConnectionString("NorthwindConnection");
            builder.UseSqlServer(connectionString);
            return new NorthwindUserContext(builder.Options);
        }
    }
}
