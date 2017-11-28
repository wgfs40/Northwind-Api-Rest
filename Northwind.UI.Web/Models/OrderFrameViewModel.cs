using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Northwind.UI.Web.Models
{
    public class OrderFrameViewModel
    {
        public string Address { get; private set; } = string.Empty;

        public List<string> Claims { get; set; } = new List<string>();

        public OrderFrameViewModel(string address,List<string>claims = null)
        {
            this.Address = address;
            this.Claims = claims;
        }
    }
}
