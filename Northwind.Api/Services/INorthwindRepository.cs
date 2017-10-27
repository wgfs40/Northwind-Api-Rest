using Northwind.Api.Entities;
using Northwind.Api.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Northwind.Api.Services
{
    public interface INorthwindRepository
    {
        PagedList<Customer> GetCustomers(CustomerResourceParameters customerResourceParameters);
        IEnumerable<Customer> GetCustomers(IEnumerable<string>customerid);
        Customer GetCustomer(string customerid);
        void AddCustomer(Customer customer);
        void DeleteCustomer(Customer customer);
        void UpdateCustomer(Customer customer);
        bool CustomerExists(string customerid);
        IEnumerable<Order> GetOrdersForCustomer(string customerid);
        Order GetOrderForCustomer(string customerid, int orderid);
        void AddOrderForCustomer(string customerid, Order order);
        void UpdateOrderForCustomer(Order order);
        void DeleteOrder(Order order);
        bool Save();

    }
}
