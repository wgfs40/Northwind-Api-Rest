using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Northwind.IDP.Entities
{
    [Table("Users",Schema ="dbo")]
    public class User
    {
        [Key]
        [MaxLength(50)]       
        public string SubjectId { get; set; }
    
        [MaxLength(100)]
        [Required]
        public string Username { get; set; }

        [MaxLength(100)]
        public string Password { get; set; }

        [Required]
        public bool IsActive { get; set; }

        public string Email { get; set; }

        public int TipoDocumento { get; set; }

        public string Documento { get; set; }

        public ICollection<UserClaim> Claims { get; set; } = new List<UserClaim>();

        public ICollection<UserLogin> Logins { get; set; } = new List<UserLogin>();


    }
}
