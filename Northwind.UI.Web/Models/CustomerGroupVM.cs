using Northwind.UI.Web.Helpers;
using PagedList.Core;

namespace Northwind.UI.Web.Models
{
    public class CustomerGroupVM
    {
        public IPagedList<CustomerDto> CustomerGroup { get; set; }
        public PagingInfo pagingInfo { get; set; }
    }
}
