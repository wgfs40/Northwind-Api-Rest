using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;

namespace Northwind.UI.Web
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();

            services.AddSingleton<IActionContextAccessor, ActionContextAccessor>();
            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();

            //configured authenticate in asp.net 2.0
            services.AddAuthentication(options =>
            {
                options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;//"Cookies";
                options.DefaultChallengeScheme = "oidc";
            })
            .AddCookie("Cookies")
            .AddOpenIdConnect("oidc", options =>
            {
                options.SignInScheme = "Cookies";

                options.Authority = "https://localhost:44384/";
                options.RequireHttpsMetadata = true;
                

                options.ClientId = "northwindclient";
                options.SaveTokens = true;
                options.ResponseType = "code id_token";
                options.ClientSecret = "secret";
                options.GetClaimsFromUserInfoEndpoint = true;
                options.ClaimActions.MapUniqueJsonKey("role", "role");
                options.Scope.Clear();
                options.Scope.Add("openid");
                options.Scope.Add("profile");
                options.Scope.Add("address");
                options.Scope.Add("roles");

                options.Events = new OpenIdConnectEvents()
                {
                    OnTokenValidated = tokenValidatedContext =>
                    {
                        var identity = tokenValidatedContext.Principal.Identity as ClaimsIdentity;

                        var subjectClaim = identity.Claims.FirstOrDefault(n => n.Type == "sub");


                        var newClaimsIdentity = new ClaimsIdentity(
                                tokenValidatedContext.Scheme.Name, "given_name", "role");

                        newClaimsIdentity.AddClaim(subjectClaim);


                        var ticket = new AuthenticationTicket(new ClaimsPrincipal(newClaimsIdentity), tokenValidatedContext.Properties, tokenValidatedContext.Scheme.Name);
                        tokenValidatedContext.Principal = ticket.Principal;
                        //tokenValidatedContext.Success();
                        //new ClaimsPrincipal(newClaimsIdentity),
                        //tokenValidatedContext.Properties,
                        //tokenValidatedContext.Scheme.Name
                        //);

                        //tokenValidatedContext.SecurityToken

                        //var tokenvalidatedContext = new TokenValidatedContext(Context.HttpContext, Context.Scheme, Context.Options, Context.Principal, Context.Properties);



                        return Task.FromResult(0);
                    },
                    OnUserInformationReceived = UserInformationReceivedContext =>
                    {
                        UserInformationReceivedContext.User.Remove("address");

                        //var role = UserInformationReceivedContext.User.GetValue("role").ToString();

                        //var newClaimsIdentity = new ClaimsIdentity(
                        //       UserInformationReceivedContext.Scheme.Name, "given_name", "role");
                        //var newclaim = new Claim("role", role);

                        //newClaimsIdentity.AddClaim(newclaim);

                        //UserInformationReceivedContext.Principal.AddIdentity(newClaimsIdentity);

                        return Task.FromResult(0);
                    }                    
                };

            });
            services.AddTransient<IClaimsTransformation, ClaimsTransformer>();
            
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }
            app.UseAuthentication();            
            app.UseStaticFiles();
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
