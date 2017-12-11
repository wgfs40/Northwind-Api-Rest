using IdentityServer4.Services;
using Microsoft.AspNetCore.Mvc;
using Northwind.IDP.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Northwind.IDP.Controllers.UserRegistation;
using IdentityServer4.EntityFramework.DbContexts;
using IdentityServer4.Stores;
using IdentityServer4.EntityFramework.Entities;

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
            var client = await _clientStore.FindClientByIdAsync("wind");

            ViewBag.Client = client;

            return View(listResources);
        }

        public IActionResult NewClaim()
        {            
            return PartialView("_AddClaimUser");
        }

        
        public IActionResult SaveClaim(Client clientids,string redirect,string allow, string postlogout,string clientsecret)
        {
            clientids.RedirectUris = new List<ClientRedirectUri>() { new ClientRedirectUri() { RedirectUri = redirect}}; 
            clientids.AllowedScopes = new List<ClientScope>() { new ClientScope { Scope = allow } };
            clientids.PostLogoutRedirectUris = new List<ClientPostLogoutRedirectUri>() { new ClientPostLogoutRedirectUri() { PostLogoutRedirectUri = postlogout } };
            clientids.ClientSecrets = new List<ClientSecret>() { new ClientSecret() { Value = clientsecret } };

            //informacion por default
            clientids.AllowedGrantTypes = new List<ClientGrantType>() { new ClientGrantType() { GrantType = "Hybrid" } };
            clientids.AccessTokenLifetime = 120;
            clientids.AccessTokenType = 1; //AccessTokenType.Reference;
            clientids.RequireConsent = false;
            clientids.UpdateAccessTokenClaimsOnRefresh = true;
            clientids.AllowOfflineAccess = true;


            _context.Clients.Add(clientids);

            if (_context.SaveChanges() > 0)
            {
                return Json(new { alert = "Datos guardato en la base de datos" });
            }

            return Json(new { alert = "paso algo"});

        }

        public IActionResult AddResource(IdentityResource resource)
        {
            return Json(new { result = "Datos guardados con exito!!!" });
        }
    }
}
