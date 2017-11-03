using System;
using System.Collections.Generic;
using System.Linq;
using Northwind.Api.Entities;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Northwind.Api.Helpers;
using Northwind.Api.Models;

namespace Northwind.Api.Services
{
    public class NorthwindRepository : INorthwindRepository
    {
        private NorthwindContext _northwindContext;
        private IPropertyMappingService _propertyMappingService;
        public NorthwindRepository(NorthwindContext northwindContext, IPropertyMappingService propertyMappingService)
        {
            this._northwindContext = northwindContext;
            this._propertyMappingService = propertyMappingService;
        }

        public void AddCustomer(Customer customer)
        {
            _northwindContext.Add(customer);
              
        }

        public void AddOrderForCustomer(string customerid, Order order)
        {
            var customer = GetCustomer(customerid);

            if (customer != null)
            {
                customer.Orders.Add(order);
            }
            
        }

        public bool CustomerExists(string customerid)
        {
            return _northwindContext.Customers.Any(c => c.CustomerID == customerid);
        }

        public void DeleteCustomer(Customer customer)
        {
            this._northwindContext.Customers.Remove(customer);
        }

        public void DeleteOrder(Order order)
        {
            this._northwindContext.Orders.Remove(order);
        }

        public Customer GetCustomer(string customerid)
        {
            return _northwindContext.Customers.FirstOrDefault(c => c.CustomerID == customerid);
        }

        public PagedList<Customer> GetCustomers(CustomerResourceParameters customerResourceParameters)
        {
            //var collectionBeforePaging = _northwindContext.Customers
            //    .OrderBy(c => c.CompanyName)
            //    .ThenBy(c => c.ContactName).AsQueryable();

            var collectionBeforePaging = _northwindContext.Customers
              .ApplySort(customerResourceParameters.OrderBy, 
              _propertyMappingService.GetPropertyMapping<CustomerDto,Customer>());



            if (!string.IsNullOrEmpty(customerResourceParameters.CompanyName))
            {
                //trim & ignore casing
                var companyName = customerResourceParameters.CompanyName.Trim().ToLowerInvariant();
                collectionBeforePaging = collectionBeforePaging
                                         .Where(c => c.CompanyName.ToLowerInvariant() == companyName);
            }

            if (!string.IsNullOrEmpty(customerResourceParameters.SearchQuery))
            {
                var searchQueryCustomer = customerResourceParameters.SearchQuery.Trim().ToLowerInvariant();

                collectionBeforePaging = collectionBeforePaging
                                         .Where(c => c.CompanyName.ToLowerInvariant().Contains(searchQueryCustomer) ||
                                                c.ContactName.ToLowerInvariant().Contains(searchQueryCustomer) ||
                                                c.ContactTitle.ToLowerInvariant().Contains(searchQueryCustomer));
            }


            return PagedList<Customer>.Create(collectionBeforePaging,
                                              customerResourceParameters.PageNumber,
                                              customerResourceParameters.PageSize);
        }

        public IEnumerable<Customer> GetCustomers(IEnumerable<string> customerid)
        {
            return _northwindContext.Customers.Where(c => customerid.Contains(c.CustomerID))
                 .OrderBy(c => c.CompanyName)
                 .ToList();
        }

        public Order GetOrderForCustomer(string customerid, int orderid)
        {
            var order = this._northwindContext.Orders.Where(o => o.CustomerID == customerid && o.OrderID == orderid).FirstOrDefault();
            return order;
        }

        public IEnumerable<Order> GetOrdersForCustomer(string customerid)
        {
            var order = _northwindContext.Orders.Include(o => o.Customers).Where(o => o.Customers.CustomerID == customerid).ToList();
            return order;
        }

        public bool Save()
        {
            return _northwindContext.SaveChanges() > 0;
        }

        public void UpdateCustomer(Customer customer)
        {
            
        }

        public void UpdateOrderForCustomer(Order order)
        {
           
        }
    }
}
