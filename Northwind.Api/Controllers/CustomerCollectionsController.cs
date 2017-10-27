using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;
using Northwind.Api.Entities;
using Northwind.Api.Helpers;
using Northwind.Api.Models;
using Northwind.Api.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Northwind.Api.Controllers
{
    [Route("api/customerCollections")]
    public class CustomerCollectionsController : Controller
    {
        private INorthwindRepository _northwindRepository;

        public CustomerCollectionsController(INorthwindRepository northwindRepository)
        {
            this._northwindRepository = northwindRepository;
        }

        [HttpPost]
        public IActionResult CreateCustomerCollection(
            [FromBody]IEnumerable<CustomerForCreationDto>customerCollection)
        {
            if (customerCollection == null)
            {
                return BadRequest();
            }

            var customerEntities = Mapper.Map<IEnumerable<Customer>>(customerCollection);

            foreach (var customer in customerEntities)
            {
                this._northwindRepository.AddCustomer(customer);
            }

            if (!this._northwindRepository.Save())
            {
                throw new Exception("Creating a Customer collection failed on save!.");
            }

            var customerCollectionToReturn = Mapper.Map<IEnumerable<CustomerDto>>(customerEntities);
            var CustomerAsString = string.Join(",",
                customerCollectionToReturn.Select(c => c.CustomerID));

            return CreatedAtRoute("GetCustomerCollection",
                new { CustomerID = CustomerAsString },
                customerCollectionToReturn);
        }

        [HttpGet("({CustomerID})",Name = "GetCustomerCollection")]
        public IActionResult GetCustomerCollection([ModelBinder(BinderType = typeof(ArrayModelBinder))] IEnumerable<string> CustomerID) {

            if (CustomerID == null)
            {
                return BadRequest();
            }

            var customerEntities = this._northwindRepository.GetCustomers(CustomerID);

            if (CustomerID.Count() != customerEntities.Count())
            {
                return NotFound();
            }

            var customerToRetun = Mapper.Map<IEnumerable<CustomerDto>>(customerEntities);
            return Ok(customerToRetun);

        }
    }
}
