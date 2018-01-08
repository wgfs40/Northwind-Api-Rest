namespace Northwind.IDP.Entities
{
    using Microsoft.AspNetCore.Identity;
    using System.Collections.Generic;

    public class ApplicationUser : IdentityUser<string>
    {
        public bool IsActive { get; set; }
        public int TipoDocumento { get; set; }
        public string Password { get; set; }
        public string Documento { get; set; }
    }
}
