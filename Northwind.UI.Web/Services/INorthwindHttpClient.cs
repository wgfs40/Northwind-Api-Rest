using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Northwind.UI.Web.Services
{
    public interface INorthwindHttpClient
    {
        Task<HttpClient> GetClient();
    }
}
