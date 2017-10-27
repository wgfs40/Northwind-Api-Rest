using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Northwind.Api.Models
{
    public class OrderForCreationDto
    {       
        [Required]
        public DateTime OrderDate { get; set; }
       
    }
}
