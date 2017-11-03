using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Northwind.UI.Web.Models
{
    public class CustomerDto
    {
        public string CustomerID { get; set; }
        public string CompanyName { get; set; }
        public string ContactFull { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string Region { get; set; }
        public string PostalCode { get; set; }
        public string Country { get; set; }
        public string Phone { get; set; }
        public string Fax { get; set; }
        public DateTime CreationTime { get; set; }
        public int Age { get; set; }

    }
}
