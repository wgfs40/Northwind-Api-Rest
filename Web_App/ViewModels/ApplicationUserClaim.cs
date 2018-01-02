using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Web_App.ViewModels
{
    public class ApplicationUserClaim : IdentityUserClaim<string>
    {     
        //public virtual List<ApplicationUser> Users { get; set; } 
    }
}
