

namespace Web_App.ViewModels
{
    using Microsoft.AspNetCore.Identity;
    using System.Collections.Generic;

    public class ApplicationUserRole : IdentityUserRole<string>
    {
        public virtual List<ApplicationUser> Users { get; set; }
        public virtual List<ApplicationRole> Roles { get; set; }
    }
}
