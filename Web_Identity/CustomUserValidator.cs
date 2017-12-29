using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Web_Identity
{
    public class CustomUserValidator : UserValidator<ApplicationUser>
    {
        public override async Task<IdentityResult> ValidateAsync(UserManager<ApplicationUser> manager, ApplicationUser user)
        {
            IdentityResult baseResult = await base.ValidateAsync(manager, user);
            List<IdentityError> errors = new List<IdentityError>(baseResult.Errors);

            if (!user.Email.EndsWith("@example.com"))
            {
                var otherAccount = await manager.FindByNameAsync(user.UserName);

                if (otherAccount != null && otherAccount.Id != user.Id)
                {
                    IdentityError DuplicateEmailError = Describer.DuplicateEmail(user.Email);
                    DuplicateEmailError.Description = " Email address must end with @example.com";                    
                    errors = new List<IdentityError>();
                }
               
            }

            return errors.Count > 0 ? IdentityResult.Failed(errors.ToArray()) : IdentityResult.Success;
        }
    }
}
