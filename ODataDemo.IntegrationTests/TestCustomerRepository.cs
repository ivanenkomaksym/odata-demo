using ODataDemo.Models;
using ODataDemo.Repository;

namespace ODataDemo.IntegrationTests
{
    internal class TestCustomerRepository : ICustomerRepository
    {
        public IQueryable<Customer> GetCustomers()
        {
            var mockCustomers = new List<Customer>
            {
                new()
                {
                    Id = 1,
                    Name = "Mock Customer 1",
                    UserRole = null,
                },
                new()
                {
                    Id = 2,
                    Name = "Mock Customer 2",
                    UserRole = null,
                }
            };

            return mockCustomers.AsQueryable();
        }
    }
}
