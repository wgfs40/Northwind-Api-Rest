using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Northwind.IDP.Controllers.UserRegisrtation
{
    public class UserListViewModel
    {   
        public string Username { get; set; }      
        //public string Firstname { get; set; }
        //public string Lastname { get; set; }
        public string Email { get; set; }
        public string Documento { get; set; }
        public string DocumentoId { get; set; }
    }
}
