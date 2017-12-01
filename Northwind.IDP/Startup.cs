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
using System.Reflection;
using IdentityServer4.EntityFramework.DbContexts;
using Microsoft.AspNetCore.Identity;

namespace Northwind.IDP
{
    public class Startup
    {
        public static IConfigurationRoot _configuration;

        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true, reloadOnChange: false)
                .AddEnvironmentVariables();

            _configuration = builder.Build();
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            var connectionString = "Data Source=GDSDW12;Initial Catalog=NorthwindUserDB;User ID=sa;Password=wilson11;MultipleActiveResultSets=true";
            services.AddDbContext<NorthwindUserContext>(o => o.UseSqlServer(connectionString));

            var identityServerDataDBConnectionString =
                _configuration["connectionStrings:identityServerdataDBConnectionString"];

            services.AddScoped<INorthwindUserRepository, NorthwindUserRepository>();
            

            var migrationsAssembly = typeof(Startup)
                .GetTypeInfo().Assembly.GetName().Name;

            //mvc
            services.AddMvc();

            //configuration identity server
            services.AddIdentityServer()
                .AddDeveloperSigningCredential()
                .AddNorthwindUserStore()
                .AddConfigurationStore(options =>
                {
                    //options.DefaultSchema = "token";
                    options.ConfigureDbContext = builder =>
                    {
                        builder.UseSqlServer(identityServerDataDBConnectionString,
                            sql => sql.MigrationsAssembly(migrationsAssembly));                        
                    };
                    
                })
                .AddOperationalStore(options => {
                    //options.DefaultSchema = "token";
                    options.ConfigureDbContext = builder => {
                        builder.UseSqlServer(identityServerDataDBConnectionString,
                            sql => sql.MigrationsAssembly(migrationsAssembly));
                    };
                });


                //.AddInMemoryApiResources(Config.GetApiResources())
                //.AddInMemoryIdentityResources(Config.GetIdentityResources())
                //.AddInMemoryClients(Config.GetClients());
                
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, 
            ILoggerFactory loggerFactory,NorthwindUserContext northwindUserContext,ConfigurationDbContext configurationDbContext,
            PersistedGrantDbContext persistedGrantDbContext)
        {
            loggerFactory.AddConsole();
            loggerFactory.AddDebug();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            configurationDbContext.Database.Migrate();
            configurationDbContext.EnsureSeedDataForContext();

            persistedGrantDbContext.Database.Migrate();

            northwindUserContext.Database.Migrate();
            northwindUserContext.EnsureSeedDataForContext();

            //call identity server
            app.UseIdentityServer();

            app.UseStaticFiles();

            app.UseMvcWithDefaultRoute();
        }
    }
}
