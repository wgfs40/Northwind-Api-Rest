using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using Northwind.IDP.Services;
using Northwind.IDP.Entities;
using System.Reflection;
using IdentityServer4.EntityFramework.DbContexts;
using Microsoft.AspNetCore.Identity;
using System;
using System.Security.Cryptography.X509Certificates;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace Northwind.IDP
{
    public class Startup
    {
        public static IConfigurationRoot _configuration;
        private const string _defaultTokenProviderName = "Default";

        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true, reloadOnChange: false)
                .AddEnvironmentVariables();

            _configuration = builder.Build();
        }

        //configurated Certificates
        public X509Certificate2 LoadCerticateFromStore()
        {
            //string thumPrint = "986E85B317C7B3313B9CC43648B4EB03B7D4BC0E";
            //string thumPrint = "964B07BB0C4642B55690F2DBE599E70029462BEC";
            string thumPrint = "6B7ACC520305BFDB4F7252DAEB2177CC091FAAE1";

            using (var store = new X509Store(StoreName.My,StoreLocation.LocalMachine))
            {
                store.Open(OpenFlags.ReadOnly);
                var certCollection = store.Certificates.Find(X509FindType.FindByThumbprint,thumPrint,true);

                if (certCollection.Count == 0)
                {
                    throw new Exception("the specified certificate wasn't found");
                }

                return certCollection[0];
            }
        }


        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            var connectionString = "Data Source=GDSDW12;Initial Catalog=NorthwindUserDB;User ID=sa;Password=wilson11;MultipleActiveResultSets=true";
            services.AddDbContext<NorthwindUserContext>(o => o.UseSqlServer(connectionString));

            var identityServerDataDBConnectionString =
                _configuration["connectionStrings:identityServerdataDBConnectionString"];

            services.AddIdentity<ApplicationUser, ApplicationRole>(option => {
                option.SignIn.RequireConfirmedEmail = true; //confirmar correo 
            })
        
                    .AddEntityFrameworkStores<NorthwindUserContext>()                      
                    .AddDefaultTokenProviders()
                    .AddTokenProvider<DefaultDataProtectorTokenProvider<ApplicationUser>>(_defaultTokenProviderName);

            //services.AddAuthentication(options => {
            //    options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            //    options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            //}).AddCookie();

            services.Configure<IdentityOptions>(options => {
                //password settings
                options.Password.RequireDigit = true;
                options.Password.RequiredLength = 8;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = true;
                options.Password.RequireLowercase = false;
                options.Password.RequiredUniqueChars = 6;


                //lokout settings
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(30);
                options.Lockout.MaxFailedAccessAttempts = 10;
                options.Lockout.AllowedForNewUsers = true;

                //user Settings
                options.User.RequireUniqueEmail = true;

                //Tokens Settings
                options.Tokens.PasswordResetTokenProvider = _defaultTokenProviderName;

            });

            //set time span for token expiration time
            services.Configure<DefaultDataProtectorTokenProviderOptions>(option =>
            {
                option.TokenLifespan = TimeSpan.FromMinutes(2); // asignacion del tiempo de expiracion del token
            });

            services.AddScoped<INorthwindUserRepository, NorthwindUserRepository>();

            var migrationsAssembly = typeof(Startup)
                .GetTypeInfo().Assembly.GetName().Name;

            //mvc
            services.AddMvc();

            //configuration identity server
            services.AddIdentityServer()
                .AddSigningCredential(LoadCerticateFromStore())
                //.AddDeveloperSigningCredential()
                .AddAspNetIdentity<ApplicationUser>()
                //.AddNorthwindUserStore()
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

                services.AddScoped<IEmailSender, AuthMessageSender>();
                services.AddScoped<ISmsSender, AuthMessageSender>();
                services.Configure<SMSoptions>(_configuration);

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
