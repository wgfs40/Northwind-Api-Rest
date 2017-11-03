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
using PagedList.Core;

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

                var pagedExpenseGroupsList = new StaticPagedList<CustomerDto>(model, paginInfo.CurrentPage, paginInfo.PageSize, paginInfo.TotalCount);
                var viewmodel = new CustomerGroupVM();

                viewmodel.CustomerGroup = pagedExpenseGroupsList;
                viewmodel.pagingInfo = paginInfo;

                return View(viewmodel);
            }
            else
            {
                return Content("An error occurred!");
            }
        }

        public IActionResult CreateCustomer()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateCustomer(CustomerDto customer)
        {
            try
            {
                customer.CreationTime = DateTime.Now;
                var client = NorthwindTrackerHttpClient.GetClient();

                var serealizeItemToCreate = JsonConvert.SerializeObject(customer);

                var response = await client.PostAsync($"api/customers",
                    new StringContent(serealizeItemToCreate,
                    System.Text.Encoding.Unicode,"application/json"));

                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    return Content("An error occurred.");
                }
            }
            catch (Exception)
            {

                return Content("An error occurred.");
            }
        }

        public async Task<IActionResult> Edit(string id)
        {
            var client = NorthwindTrackerHttpClient.GetClient();

            HttpResponseMessage response = await client.GetAsync($"api/customers/"+id);

            if (response.IsSuccessStatusCode)
            {
                string content = await response.Content.ReadAsStringAsync();
                var model = JsonConvert.DeserializeObject<CustomerForCreationDto>(content);
                return View(model);
            }
            return Content("An error occurred!");
        }

        [HttpPost]
        public async Task<IActionResult> Edit(string id, CustomerForCreationDto customer)
        {
            try
            {
                var client = NorthwindTrackerHttpClient.GetClient();
                //serealize & PUT
                var serealizedItemToUpdate = JsonConvert.SerializeObject(customer);

                var response = await client.PutAsync($"api/customers/" + id,
                    new StringContent(serealizedItemToUpdate,System.Text.Encoding.Unicode,"application/json"));

                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("CustomerList");
                }
                else
                {
                    return Content("An error occurred!");
                }
            }
            catch (Exception)
            {
                return Content("An error occurred!");
            }
            
        }
    }
}
