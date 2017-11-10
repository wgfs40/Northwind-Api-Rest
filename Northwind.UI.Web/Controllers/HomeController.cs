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
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Authorization;
using IdentityModel.Client;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.AspNetCore.Authentication;

namespace Northwind.UI.Web.Controllers
{
    [Authorize]
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
        public async Task<IActionResult> CreateCustomer(CustomerForCreationDto customer)
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

                //partial edit
                JsonPatchDocument<CustomerForCreationDto> patchDocument = new JsonPatchDocument<CustomerForCreationDto>();
                patchDocument.Replace(eg => eg.CompanyName,customer.CompanyName);
                patchDocument.Replace(eg => eg.ContactName, customer.ContactName);
                patchDocument.Replace(eg => eg.ContactTitle, customer.ContactTitle);

                var serializedItemToUpdate = JsonConvert.SerializeObject(patchDocument);

                var response = await client.PatchAsync($"api/customers/" + id,
                    new StringContent(serializedItemToUpdate, System.Text.Encoding.Unicode, "application/json"));

                ////serealize & PUT
                //var serealizedItemToUpdate = JsonConvert.SerializeObject(customer);

                //var response = await client.PutAsync($"api/customers/" + id,
                //    new StringContent(serealizedItemToUpdate,System.Text.Encoding.Unicode,"application/json"));

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

        public async Task<IActionResult> Delete(string id)
        {
            try
            {
                var client = NorthwindTrackerHttpClient.GetClient();
                var response = await client.DeleteAsync($"api/customers/" + id);

                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("CustomerList");
                }
                else
                {
                    return Content("an error occurred!!");
                }
            }
            catch (Exception)
            {
                return Content("an error occurred!!");
            }            
        }

        public async Task Logout()
        {            
            await AuthenticationHttpContextExtensions.SignOutAsync(HttpContext,"Cookies");
            await AuthenticationHttpContextExtensions.SignOutAsync(HttpContext, "oidc");
            //await HttpContext.Authentication.SignOutAsync("Cookies");
        }

        public async Task<IActionResult> OrderFrame()
        {
            var discoveryClient = new DiscoveryClient("https://localhost:44384/");
            var metaDataResponse = await discoveryClient.GetAsync();

            var userInfoClient = new UserInfoClient(metaDataResponse.UserInfoEndpoint);

            var accessToken = await AuthenticationHttpContextExtensions.GetTokenAsync(HttpContext,OpenIdConnectParameterNames.AccessToken);

            var response = await userInfoClient.GetAsync(accessToken);

            if (response.IsError)
            {
                throw new Exception(
                        "Problem accessing the UserInfo endpoint.",
                        response.Exception
                    );
            }

            var address = response.Claims.FirstOrDefault(c => c.Type == "address")?.Value;

            return View(new OrderFrameViewModel(address));
        }
    }
}
