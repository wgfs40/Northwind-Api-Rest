using Repository.Pattern.Repositories;
using Service.Pattern;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Web_Identity.Models;
using Web_Identity.Repositories;

namespace Web_Identity.Services
{
    public interface ICustomerService : IService<Customer>
    {
        decimal CustomerOrderTotalByYear(string customerId, int year);
        IEnumerable<Customer> CustomersByCompany(string companyName);
        //IEnumerable<CustomerOrder> GetCustomerOrder(string country);
    }
    public class CustomerService : Service<Customer>, ICustomerService
    {
        private readonly IRepositoryAsync<Customer> _repository;

        public CustomerService(IRepositoryAsync<Customer> repository)
            :base(repository)
        {
            this._repository = repository;
        }

        public decimal CustomerOrderTotalByYear(string customerId, int year)
        {
            return _repository.GetCustomerOrderTotalByYear(customerId,year);
        }

        public IEnumerable<Customer> CustomersByCompany(string companyName)
        {
            throw new NotImplementedException();
        }
    }
}
