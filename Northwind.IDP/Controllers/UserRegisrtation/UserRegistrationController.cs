using IdentityServer4.Services;
using Microsoft.AspNetCore.Mvc;
using Northwind.IDP.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http.Authentication;
using Microsoft.AspNetCore.Http;
using Northwind.IDP.Controllers.UserRegistation;

namespace Northwind.IDP.Controllers.UserRegistration
{
    public class UserRegistrationController:Controller
    {
        private readonly INorthwindUserRepository _northwindRepository;
        private readonly IIdentityServerInteractionService _interaction;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserRegistrationController(INorthwindUserRepository northwindRepository,
            IIdentityServerInteractionService interaction, IHttpContextAccessor httpContextAccessor)
        {
            this._northwindRepository = northwindRepository;
            this._interaction = interaction;
            this._httpContextAccessor = httpContextAccessor;
        }

        [HttpGet]
        public IActionResult RegisterUser(string returnUrl)
        {
            var vm = new RegisterUserViewModel()
            {
                ReturnUrl = returnUrl
            };
            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RegisterUser(RegisterUserViewModel model)
        {
            if (ModelState.IsValid)
            {
                //create user + claims
                var usertoCreate = new Entities.User();
                usertoCreate.Password = model.Password;
                usertoCreate.Username = model.Username;
                usertoCreate.TipoDocumento = int.Parse(model.DocumentoId.ToString());
                usertoCreate.Documento = model.Documento;
                usertoCreate.Email = model.Email;
                usertoCreate.IsActive = true;



                usertoCreate.Claims.Add(new Entities.UserClaim("country", model.Country));
                usertoCreate.Claims.Add(new Entities.UserClaim("address", model.Address));
                usertoCreate.Claims.Add(new Entities.UserClaim("given_name", model.Firstname));
                usertoCreate.Claims.Add(new Entities.UserClaim("family_name", model.Lastname));
                usertoCreate.Claims.Add(new Entities.UserClaim("email", model.Email));
                usertoCreate.Claims.Add(new Entities.UserClaim("subscriptionlevel", "FreeUser"));

                // add it through the repository
                _northwindRepository.AddUser(usertoCreate);

                if (!_northwindRepository.Save())
                {
                    throw new Exception($"Creating a user failed.");
                }

                // log the user in 
                await _httpContextAccessor.HttpContext.SignInAsync(usertoCreate.SubjectId, usertoCreate.Username);

                // continue with the flow
                if (_interaction.IsValidReturnUrl(model.ReturnUrl) || Url.IsLocalUrl(model.ReturnUrl))
                {
                    return Redirect(model.ReturnUrl);
                }

                return Redirect("~/");
            }

            // modelState invalid, return the view with the passed-in model
            // so changes can be made
            return View(model);
        }
    }
}
