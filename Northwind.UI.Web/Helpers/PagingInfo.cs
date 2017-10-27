using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Northwind.UI.Web.Helpers
{
    public class PagingInfo
    {
        public int TotalCount { get; set; }
        public int TotalPages { get; set; }

        public int CurrentPage { get; set; }
        public int PageSize { get; set; }
        public string PreviousPagelink { get; set; }
        public string NextPageLink { get; set; }

        public PagingInfo(int totalcount,int totalpages,int currentpage,
            int pagesize, string previousPagelink,string nextPageLink)
        {
            this.TotalCount = totalcount;
            this.TotalPages = totalpages;
            this.CurrentPage = currentpage;
            this.PageSize = pagesize;
            this.PreviousPagelink = previousPagelink;
            this.NextPageLink = nextPageLink;
        }
    }
}
