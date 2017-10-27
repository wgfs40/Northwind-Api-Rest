using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Northwind.Api.Entities
{
    public class Order
    {
        [Key]
        public int OrderID { get; set; }      
        
        [Required]
        public DateTime OrderDate { get; set; }

        public string CustomerID { get; set; }

        [ForeignKey("CustomerID")]
        public Customer Customers { get; set; }
    }
}
