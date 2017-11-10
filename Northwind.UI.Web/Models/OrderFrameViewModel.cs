using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Northwind.UI.Web.Models
{
    public class OrderFrameViewModel
    {
        public string Address { get; private set; } = string.Empty;

        public OrderFrameViewModel(string address)
        {
            this.Address = address;
        }
    }
}
