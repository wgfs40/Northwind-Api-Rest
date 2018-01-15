using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Northwind_Web.ViewModels
{
    public class RegisterModel
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string FullName { get; set; }
        public string AddressLine { get; set; }
        public string Country { get; set; }
    }
}