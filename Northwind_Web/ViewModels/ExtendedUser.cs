namespace Northwind_Web.ViewModels
{
    using Microsoft.AspNet.Identity.EntityFramework;
    using System;
    using System.Collections.Generic;

    public class ExtendedUser : IdentityUser
    {
        public ExtendedUser()
        {
            Addresses = new List<Address>();
        }
        public string FullName { get; set; }
        public virtual ICollection<Address> Addresses { get; set; }

    }

    public class Address
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string UserId { get; set; }
        public string AddressLine { get; set; }
        public string Country { get; set; }
    }
}