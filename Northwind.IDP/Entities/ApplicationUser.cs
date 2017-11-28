using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Northwind.IDP.Entities
{
    public class ApplicationUser : IdentityUser
    {
        [MaxLength(100)]
        public string Password { get; set; }
        [Required]
        public bool IsActive { get; set; }
        public int TipoDocumento { get; set; }
        public string Documento { get; set; }
    }
}
