using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Northwind.UI.Web.Helpers
{
    public static class HttpClientExtensions
    {
        public static Task<HttpResponseMessage> PatchAsync(this HttpClient client,string requestUri,
            HttpContent content)
        {
            var method = new HttpMethod("PATCH");
            var resquest = new HttpRequestMessage(method, requestUri)
            {
                Content = content
            };

            return client.SendAsync(resquest);
        }

        public static Task<HttpResponseMessage> PatchAsync(this HttpClient client, Uri requestUri,
            HttpContent content)
        {
            var method = new HttpMethod("PATCH");
            var resquest = new HttpRequestMessage(method, requestUri)
            {
                Content = content
            };

            return client.SendAsync(resquest);
        }

        public static Task<HttpResponseMessage> PatchAsync(this HttpClient client,Uri requestUri,
            HttpContent content, CancellationToken cancellationToken)
        {
            var method = new HttpMethod("PATCH");

            var request = new HttpRequestMessage(method, requestUri) {
                Content = content
            };

            return client.SendAsync(request, cancellationToken);
        }

        public static Task<HttpResponseMessage> PatchAsync(this HttpClient client, string requestUri,
            HttpContent content, CancellationToken cancellationToken)
        {
            var method = new HttpMethod("PATCH");

            var request = new HttpRequestMessage(method, requestUri)
            {
                Content = content
            };

            return client.SendAsync(request, cancellationToken);
        }
    }
}
