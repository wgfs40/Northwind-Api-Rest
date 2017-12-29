using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Web_App.ViewModels;

namespace Web_App.Controllers
{
    public class AccountController : Controller
    {
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Register(ApplicationUser model)
        {
            if (ModelState.IsValid)
            {

            }
            return View();
        }
    }
}
