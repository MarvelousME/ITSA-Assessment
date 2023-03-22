using System.Collections.Generic;
using VetClinic.DAl.Models;
using VetClinic.DAL.Repositories.Interfaces;

namespace VetClinic.DAl.Repositories.Interfaces
{
    public interface ICustomerRepository : IRepository<Customer>
    {
        IEnumerable<Customer> GetTopActiveCustomers(int count);
        IEnumerable<Customer> GetAllCustomersData();
    }
}
