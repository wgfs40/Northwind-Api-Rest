using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace Northwind.UI.Web.Helpers
{
    public static class HeaderParse
    {
      public static PagingInfo FindAndParsePagingInfo(HttpResponseHeaders httpResponseHeaders)
        {
            //find the X-pagination info in header
            if (httpResponseHeaders.Contains("X-Pagination"))
            {
                var xPag = httpResponseHeaders.First(ph => ph.Key == "X-Pagination").Value;

                //parse Value - this is a JSon-string
                return JsonConvert.DeserializeObject<PagingInfo>(xPag.First());
            }

            return null;

        }
    }
}
