
using IdentityModel.Client;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Northwind.UI.Web.Controllers
{
    public class AuthorizationController : Controller
    {
        public IActionResult AccessDenied()
        {
            return View();
        }

        public async Task<IActionResult> AccessToken()
        {
            //get data metada
            var discoveryClient = new DiscoveryClient("https://localhost:44384/");
            var metaDataResponse = await discoveryClient.GetAsync();

            // get the access token to revoke
            var accessToken = await HttpContext.GetTokenAsync(OpenIdConnectParameterNames.AccessToken);

            ViewBag.token = accessToken;

            return View();
        }
    }
}
