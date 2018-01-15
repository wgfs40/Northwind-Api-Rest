using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using System.Web.Mvc;
using System.Web;
using Northwind_Web.ViewModels;
using System.Threading.Tasks;
using System.Linq;

namespace Northwind_Web.Controllers
{
    public class AccountController : Controller
    {
        public UserManager<ExtendedUser> UserManager => HttpContext.GetOwinContext().Get<UserManager<ExtendedUser>>();
        public SignInManager<ExtendedUser, string> signInManager => HttpContext.GetOwinContext().Get<SignInManager<ExtendedUser, string>>();

        // GET: Account
        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Register(RegisterModel model)
        {
            var identityUser = await UserManager.FindByNameAsync(model.Username);
            if (identityUser != null)
            {
                return RedirectToAction("Index","Home");
            }

            var user = new ExtendedUser {
                UserName = model.Username,
                FullName = model.FullName
            };
            user.Addresses.Add(new Address { AddressLine = model.AddressLine, Country = model.Country , UserId = user.Id });
           var identityResult = await UserManager.CreateAsync(user, model.Password);

            if (identityResult.Succeeded)
            {
                return RedirectToAction("Index", "Home");
            }

            ModelState.AddModelError("", identityResult.Errors.FirstOrDefault());

            return View(model);
        }

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Login(LoginModel model)
        {
           var signinStatus =  await signInManager.PasswordSignInAsync(model.Username, model.Password, true, true);
            switch (signinStatus)
            {
                case SignInStatus.Success:
                    return RedirectToAction("Index", "Home");                                    
                default:
                    ModelState.AddModelError("","Invalid Credentials!!");
                    return View(model);                    
            }            
        }
     
        public ActionResult Logout()
        {
            var authenticationManager = HttpContext.GetOwinContext().Authentication;
            authenticationManager.SignOut();
            return View("Logout");
        }
    }
}