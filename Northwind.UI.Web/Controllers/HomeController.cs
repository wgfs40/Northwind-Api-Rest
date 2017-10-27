using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Northwind.UI.Web.Models;
using Northwind.UI.Web.Helpers;
using System.Net.Http;
using Newtonsoft.Json;

namespace Northwind.UI.Web.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public async Task<IActionResult> CustomerList(int page = 1)
        {
            var client = NorthwindTrackerHttpClient.GetClient();
            HttpResponseMessage httpResponse = await client.GetAsync($"api/customers?pageNumber={page}&pageSize=10");

            if (httpResponse.IsSuccessStatusCode)
            {
                string content = await httpResponse.Content.ReadAsStringAsync();

                var paginInfo = HeaderParse.FindAndParsePagingInfo(httpResponse.Headers);

                
                var model = JsonConvert.DeserializeObject<IEnumerable<CustomerDto>>(content);
                return View(model);
            }
            else
            {
                return Content("An error occurred!");
            }
        }
    }
}
