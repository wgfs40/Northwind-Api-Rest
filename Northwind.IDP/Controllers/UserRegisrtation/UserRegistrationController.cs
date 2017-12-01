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
using Microsoft.AspNetCore.Identity;
using Northwind.IDP.Entities;
using IdentityServer4.EntityFramework.DbContexts;
using IdentityServer4.Stores;
using System.Threading;

namespace Northwind.IDP.Controllers.UserRegistration
{
    public class UserRegistrationController:Controller
    {
        private readonly INorthwindUserRepository _northwindRepository;
        private readonly IIdentityServerInteractionService _interaction;
        private readonly IHttpContextAccessor _httpContextAccessor;
        //private readonly UserManager<ApplicationUser> _userManager;
        private ConfigurationDbContext _context;
        private readonly IClientStore _clientStore;
        private readonly IResourceStore _resourceStore;        

        public UserRegistrationController(INorthwindUserRepository northwindRepository,
            IIdentityServerInteractionService interaction, IHttpContextAccessor httpContextAccessor,
            ConfigurationDbContext context, IClientStore clientStore, IResourceStore resourceStore)
        {
            this._northwindRepository = northwindRepository;
            this._interaction = interaction;
            this._httpContextAccessor = httpContextAccessor;
            this._context = context;
            this._clientStore = clientStore;
            this._resourceStore = resourceStore;            
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

                //var user = new ApplicationUser {
                //     UserName = model.Email,
                //     Email = model.Email,
                //     IsActive = true,
                //     TipoDocumento = int.Parse(model.DocumentoId.ToString()),
                //     Documento = model.Documento
                //};

                //await _userManager.AddClaimAsync(user, new System.Security.Claims.Claim("country", model.Country));
                //await _userManager.AddClaimAsync(user, new System.Security.Claims.Claim("address", model.Address));
                //await _userManager.AddClaimAsync(user, new System.Security.Claims.Claim("given_name", model.Firstname));
                //await _userManager.AddClaimAsync(user, new System.Security.Claims.Claim("family_name", model.Lastname));
                //await _userManager.AddClaimAsync(user, new System.Security.Claims.Claim("email", model.Email));
                //await _userManager.AddClaimAsync(user, new System.Security.Claims.Claim("subscriptionlevel", "FreeUser"));


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

                //add it through the repository
                _northwindRepository.AddUser(usertoCreate);

                //var result = await _userManager.CreateAsync(user, model.Password);

                //if (result.Errors.Count() > 0)
                //{
                //    throw new Exception($"Creating a user failed.");
                //}

                //if (result.Succeeded)
                //{                    
                //   await _httpContextAccessor.HttpContext.SignInAsync(user.Id, user.UserName);
                //}

                //if (_interaction.IsValidReturnUrl(model.ReturnUrl) || Url.IsLocalUrl(model.ReturnUrl))
                //{
                //    return Redirect(model.ReturnUrl);
                //}


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

        public IActionResult UserList()
        {
            var userlist = _northwindRepository.GetUserListActive();

            return View(userlist);
        }

        public IActionResult Details(string id)
        {
            var userClaim = _northwindRepository.GetUserClaimsBySubjectId(id);
            ViewBag.user = _northwindRepository.GetUserBySubjectId(id).Email;
            
            return View(userClaim);
        }

        public async Task<IActionResult> Resources()
        {
            var listResources = await _resourceStore.GetAllResourcesAsync();
            var user = _northwindRepository.GetUserByUsername("wfernandez@wind.com.do");
            var client = await _clientStore.FindClientByIdAsync("northwindclient");

            ViewBag.Client = client;

            return View(listResources);
        }
    }
}
