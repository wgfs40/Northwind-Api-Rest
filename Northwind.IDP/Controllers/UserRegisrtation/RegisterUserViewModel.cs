using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace Northwind.IDP.Controllers.UserRegistation
{
    public class RegisterUserViewModel
    {
        //credentials
        [MaxLength(100)]
        public string Username { get; set; }
        [MaxLength(100)]
        [DataType(DataType.Password)]
        [Required]
        public string Password { get; set; }
        [Compare("Password",ErrorMessage ="El password no es igual, favor de confirmar")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }
        //claims
        [Required]
        [MaxLength(100)]
        public string Firstname { get; set; }
        [Required]
        [MaxLength(100)]
        public string Lastname { get; set; }
        [Required]
        [MaxLength(150)]
        public string Email { get; set; }
        [Required]
        [MaxLength(200)]
        public string Address { get; set; }
        [Required]
        [MaxLength(2)]
        public string Country { get; set; }
        public SelectList Countrycodes { get; set; } =
            new SelectList(
                new[]
                {
                    new{ Id = "BE", Value="Belgium"},
                    new{ Id = "US", Value="United States of America"},
                    new{ Id="RD", Value="Dominican Republic"}},"Id","Value");

        public string Documento { get; set; }
        public string DocumentoId { get; set; }
        public SelectList TipoDocumento { get; set; } =
            new SelectList(new[] {
                new{ Id = "1", Value="Cedula"},
                    new{ Id = "2", Value="Passaporte"},
                    new{ Id="3", Value="RNC"}}, "Id", "Value");

        public string ReturnUrl { get; set; }

        public string CaptchaCode { get; set; }
    }
}
