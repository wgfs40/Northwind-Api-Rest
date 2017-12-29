namespace Web_Identity.Controllers
{
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
    using Microsoft.AspNetCore.Mvc;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;    
    using System.Text;
    using Web_Identity.ViewModel;
    using System.Security.Cryptography;
    using System.Configuration;
    using Microsoft.AspNetCore.Cryptography.KeyDerivation;
    using System.Security.Claims;

    public class AccountController : Controller
    {        
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;

        public AccountController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager)
        {
            this._userManager = userManager;
            this._signInManager = signInManager;
        }
       public IActionResult UserCreate()
        {
            return View(new RegisterViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> UserCreate(RegisterViewModel user)
        {
            if (ModelState.IsValid)
            {
                var userCreateResult = await this._userManager.CreateAsync(new IdentityUser()
                {
                    UserName = user.Email,
                    Email = user.Email
                    
                }, user.Password);


                if (userCreateResult.Succeeded)
                {
                    return RedirectToAction("Index", "Home");
                    
                }

                var errorsuser = userCreateResult.Errors.Where(x => x.Code != "DuplicateUserName").ToList();
                if (errorsuser.Count() > 0)
                {
                    foreach (var erro in errorsuser)
                    {
                        switch (erro.Code)
                        {
                            case "DuplicateEmail":
                                erro.Description = "El Correo ya existe, favor de registrar otro!!";
                                break;
                            case "":
                                erro.Description = "";
                                break;
                            default:
                                break;
                        }

                        ModelState.AddModelError("", erro.Description);
                    }

                }

            }
            return View();
        }

        public IActionResult Login()
        {
            return View(new LoginViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                 password: model.Password,
                 salt: new byte[128 / 8],
                 prf: KeyDerivationPrf.HMACSHA1,
                 iterationCount: 10000,
                 numBytesRequested: 256 / 8));

                var userm = await _userManager.FindByNameAsync(model.Email);

                var result = await _signInManager.PasswordSignInAsync(userm, model.Password,false,true);
                
                
                if (result.Succeeded)
                {
                    return RedirectToAction("Index", "Home");
                }
            }
            return View();
        }

        
        public async Task<IActionResult> AddClaim()
        {
            var finduser = await _userManager.FindByNameAsync("wgfs40@gmail.com");
            var resulclaim = await _userManager.AddClaimAsync(finduser, new Claim("given_name", "Servidor"));

            if (resulclaim.Succeeded)
            {
                return Json(new { data = "Claim totalmente adentro" });
            }

            return Json(new{ data = "saludos, ha fallado" });
        }
    }
}
