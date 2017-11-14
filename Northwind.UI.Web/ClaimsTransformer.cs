using Microsoft.AspNetCore.Authentication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;

namespace Northwind.UI.Web
{
    public class ClaimsTransformer : IClaimsTransformation
    {
        //private UserManager<IdentityUser> _userManager;
        //private IUserClaimsPrincipalFactory<IdentityUser> _userClaimsPrincipalFactory;

        //public ClaimsTransformer(IUserClaimsPrincipalFactory<IdentityUser> userClaimsPrincipalFactory)
        //{
        //    this._userClaimsPrincipalFactory = userClaimsPrincipalFactory;
        //}
        public async Task<ClaimsPrincipal> TransformAsync(ClaimsPrincipal principal)
        {

            var user = principal.Identity.Name;

            var identity = principal.Identity;

           
            var name = principal.Identity.Name;
            var role = principal.Claims;
           

            //principal.Identities.First().AddClaim(new Claim("now", DateTime.Now.ToString()));
            return await Task.FromResult(principal);
        }
    }
}
