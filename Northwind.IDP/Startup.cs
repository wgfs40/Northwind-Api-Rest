using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using Northwind.IDP.Services;
using Northwind.IDP.Entities;

namespace Northwind.IDP
{
    public class Startup
    {
        public static IConfigurationRoot _configuration;
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            var connectionString = "Data Source=GDSDW12;Initial Catalog=NorthwindUserDB;User ID=sa;Password=wilson11;MultipleActiveResultSets=true";
            services.AddDbContext<NorthwindUserContext>(o => o.UseSqlServer(connectionString));

            services.AddScoped<INorthwindUserRepository, NorthwindUserRepository>();

            //mvc
            services.AddMvc();

            //configuration identity server
            services.AddIdentityServer()
                .AddDeveloperSigningCredential()
                .AddNorthwindUserStore()
                .AddInMemoryApiResources(Config.GetApiResources())
                .AddInMemoryIdentityResources(Config.GetIdentityResources())
                .AddInMemoryClients(Config.GetClients());
                
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, 
            ILoggerFactory loggerFactory,NorthwindUserContext northwindUserContext)
        {
            loggerFactory.AddConsole();
            loggerFactory.AddDebug();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            northwindUserContext.Database.Migrate();
            northwindUserContext.EnsureSeedDataForContext();

            //call identity server
            app.UseIdentityServer();

            app.UseStaticFiles();

            app.UseMvcWithDefaultRoute();
        }
    }
}
