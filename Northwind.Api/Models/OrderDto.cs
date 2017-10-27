using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Northwind.Api.Models
{
    public class OrderDto
    {
        public int OrderID { get; set; }
        public DateTime OrderDate { get; set; }
        public string CustomerID { get; set; }

    }
}
