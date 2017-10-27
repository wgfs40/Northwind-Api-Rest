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

        public int Age { get; set; }

    }
}
