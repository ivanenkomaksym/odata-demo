using ODataDemo.Models;

namespace ODataDemo.Repository
{
    public interface ICustomerRepository
    {
        IQueryable<Customer> GetCustomers();
    }
}
