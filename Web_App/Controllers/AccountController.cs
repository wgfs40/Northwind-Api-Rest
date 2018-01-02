namespace Web_App.Controllers
{
    using Microsoft.AspNetCore.Authentication;
    using Microsoft.AspNetCore.Authentication.Cookies;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using System.Linq;
    using System.Security.Claims;
    using System.Threading.Tasks;
    using Web_App.ViewModels;

    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        

        public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            this._userManager = userManager;
            this._signInManager = signInManager;
        }
        public IActionResult Register()
        {            
            return View(new RegisterViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var userCreate = await _userManager.CreateAsync(new ApplicationUser() {
                    UserName = model.Email,
                    Email = model.Email,
                },model.Password);

                if (userCreate.Succeeded)
                {
                    var user = await _userManager.FindByNameAsync(model.Email);
                    var claim = await _userManager.AddClaimAsync(user, new Claim("given_name", "Servidor"));

                    return RedirectToAction("Index","Home");
                }

                if (userCreate.Errors.Count() > 0)
                {
                    foreach (var error in userCreate.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                }
            }
            return View();
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByNameAsync(model.Email);

                var login = await _signInManager.PasswordSignInAsync(user,model.Password,true,true);

                if (login.Succeeded)
                {
                    if (login.IsLockedOut)
                    {
                        ModelState.AddModelError("","User LockedOut!!");
                        return View();
                    }
                    var identity = await _signInManager.CreateUserPrincipalAsync(user);
                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,new ClaimsPrincipal(identity));
                   
                    return RedirectToAction("Index", "Home");
                }
            }

            return View();
        }

       public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
    }
}
