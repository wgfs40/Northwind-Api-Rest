using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Northwind.UI.Web.Helpers
{
    public static class NorthwindTrackerHttpClient
    {
        public static HttpClient GetClient(string requestedVersion = null)
        {
            HttpClient client = new HttpClient();

            if (requestedVersion != null)
            {
                //through a custom request header
                client.DefaultRequestHeaders.Add("api-version",requestedVersion);
            }

            client.BaseAddress = new Uri("http://localhost:59479/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(
                new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

            return client;

        }
    }
}
