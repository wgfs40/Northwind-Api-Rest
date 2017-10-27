using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Northwind.UI.Web.Helpers
{
    public static class NorthwindTrackerHttpClient
    {
        public static HttpClient GetClient()
        {
            HttpClient client = new HttpClient();

            client.BaseAddress = new Uri("http://localhost:59479/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(
                new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

            return client;

        }
    }
}
