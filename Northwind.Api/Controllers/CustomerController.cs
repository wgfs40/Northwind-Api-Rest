﻿using Microsoft.AspNetCore.Mvc;
using Northwind.Api.Models;
using Northwind.Api.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Northwind.Api.Helpers;
using AutoMapper;
using Northwind.Api.Entities;
using Microsoft.AspNetCore.Http;

namespace Northwind.Api.Controllers
{
    [Route("api/customers")]
    public class CustomerController : Controller
    {
        private INorthwindRepository _northwindRepository;
        private IUrlHelper _urlHelper;
        public CustomerController(INorthwindRepository northwindRepository, IUrlHelper urlHelper)
        {
            this._northwindRepository = northwindRepository;
            this._urlHelper = urlHelper;
        }

        [HttpGet(Name = "GetCustomers")]
        public IActionResult GetCustomers(CustomerResourceParameters customerResourceParameters)
        {
           var CustomerFromRepo = _northwindRepository.GetCustomers(customerResourceParameters);

            var previousPageLink = CustomerFromRepo.HasPrevious ?
                CreateCustomersResourceUri(customerResourceParameters, ResourceUriType.PreviousPage) : null;

            var nextPageLink = CustomerFromRepo.HasNext ?
                               CreateCustomersResourceUri(customerResourceParameters, ResourceUriType.NextPage) : null;

            var paginationMetada = new
            {
                totalCount = CustomerFromRepo.TotalCount,
                pageSize = CustomerFromRepo.PageSize,
                currentPage = CustomerFromRepo.CurrentPage,
                totalPages = CustomerFromRepo.TotalPage,
                previousPageLink = previousPageLink,
                nextPageLink = nextPageLink
            };

            Response.Headers.Add("X-Pagination",
                Newtonsoft.Json.JsonConvert.SerializeObject(paginationMetada));

           var customerDto = Mapper.Map<IEnumerable<CustomerDto>>(CustomerFromRepo);
           return Ok(customerDto);
        }

        private string CreateCustomersResourceUri(
            CustomerResourceParameters customerResourceParameters,
            ResourceUriType type)
        {
            switch (type)
            {
                case ResourceUriType.PreviousPage:
                    return _urlHelper.Link("GetCustomers",
                        new {
                            searchQuery = customerResourceParameters.SearchQuery,
                            companyName = customerResourceParameters.CompanyName,
                            pageNumber = customerResourceParameters.PageNumber - 1,
                            pageSize = customerResourceParameters.PageSize
                        });                    
                case ResourceUriType.NextPage:
                    return _urlHelper.Link("GetCustomers",
                        new
                        {
                            searchQuery = customerResourceParameters.SearchQuery,
                            companyName = customerResourceParameters.CompanyName,
                            pageNumber = customerResourceParameters.PageNumber + 1,
                            pageSize = customerResourceParameters.PageSize
                        });
                default:
                    return _urlHelper.Link("GetCustomers",
                       new
                       {
                           searchQuery = customerResourceParameters.SearchQuery,
                           companyName = customerResourceParameters.CompanyName,
                           pageNumber = customerResourceParameters.PageNumber,
                           pageSize = customerResourceParameters.PageSize
                       });
            }
        }

        [HttpGet("{CustomerID}",Name ="GetCustomer")]
        public IActionResult GetCustomer(string CustomerID)
        {
            var CustomerFromRepo = this._northwindRepository.GetCustomer(CustomerID);
            if (CustomerFromRepo == null)
            {
                return NotFound();
            }
            var customer = Mapper.Map<CustomerDto>(CustomerFromRepo);
            return Ok(customer);
        }

        [HttpPost]
        public IActionResult CreateCustomer([FromBody]CustomerForCreationDto customer) {

            if (customer == null)
            {
                return BadRequest();
            }

            var CustomerEntity = Mapper.Map<Customer>(customer);
            this._northwindRepository.AddCustomer(CustomerEntity);

            if (!this._northwindRepository.Save())
            {
                return StatusCode(500, "A problem happened with handling your resquest.");
            }

            var CustomerToReturn = Mapper.Map<CustomerDto>(CustomerEntity);
            return CreatedAtRoute("Getcustomer",
                new { CustomerID  = CustomerToReturn.CustomerID},
                CustomerToReturn);
        }

        [HttpPost("{CustomerID}")]
        public IActionResult BlockCustomerCreation(string CustomerID) {

            if (_northwindRepository.CustomerExists(CustomerID))
            {
                return new StatusCodeResult(StatusCodes.Status409Conflict);
            }

            return NotFound();

        }

        [HttpDelete("{CustomerID}")]
        public IActionResult DeleteCustomer(string CustomerID)
        {
            var customerForRepo = _northwindRepository.GetCustomer(CustomerID);

            if (customerForRepo == null)
            {
                return NotFound();
            }

            _northwindRepository.DeleteCustomer(customerForRepo);

            if (!_northwindRepository.Save())
            {
                throw new Exception($"Deleting customer {CustomerID} failed on save!.");
            }

            return NoContent();
        }
    }
}
