using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Northwind.Api.Models
{
    public class CustomerForCreationDto
    {
        public string CustomerID { get; set; }
        public string CompanyName { get; set; }
        public string ContactName { get; set; }

        public string ContactTitle { get; set; }
        public DateTime CreationTime { get; set; }

        public ICollection<OrderForCreationDto> Orders { get; set; }
            = new List<OrderForCreationDto>();
    }
}
