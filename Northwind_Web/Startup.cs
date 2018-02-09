using System;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Owin;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security.Cookies;
using Northwind_Web.Models;
using Northwind_Web.ViewModels;

[assembly: OwinStartup(typeof(Northwind_Web.Startup))]
namespace Northwind_Web
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            const string connectionString = "Data Source=GDSDW12;Initial Catalog=NorthwindUsertestDB.01;User ID=sa;Password=wilson11;MultipleActiveResultSets=true";
            app.CreatePerOwinContext(() => new ExtendedUserDbContext(connectionString));

            app.CreatePerOwinContext<UserStore<ExtendedUser>>((opt, cont) => new UserStore<ExtendedUser>(cont.Get<ExtendedUserDbContext>()));
            app.CreatePerOwinContext<UserManager<ExtendedUser>>(
                (opt, cont) => 
                    {
                        var usermanager = new UserManager<ExtendedUser>(cont.Get<UserStore<ExtendedUser>>());
                        usermanager.UserTokenProvider = new DataProtectorTokenProvider<ExtendedUser>(opt.DataProtectionProvider.Create());
                        return usermanager;
                    }
                );
                   

            app.CreatePerOwinContext<SignInManager<ExtendedUser, string>>(
                (opt,cont) => 
                new SignInManager<ExtendedUser, string>(cont.Get<UserManager<ExtendedUser>>(),cont.Authentication));

            app.UseCookieAuthentication(new CookieAuthenticationOptions {

                AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
                LoginPath = new PathString("/Account/Login"),
                LogoutPath = new PathString("/Account/Logout"),
                ExpireTimeSpan = TimeSpan.FromMinutes(1)
            });
        }
    }
}
