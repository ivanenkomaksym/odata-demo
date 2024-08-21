using ODataDemo.Models;

namespace ODataDemo.Repository
{
    public class StaticCustomerRepository : ICustomerRepository
    {
        private static readonly int nofCustomers = 10;
        private static readonly Random random = new();
        private static readonly UserRole[] userRoles = { UserRole.Admin, UserRole.User };
        private static readonly List<Customer> customers = new(
            Enumerable.Range(1, nofCustomers).Select(idx => new Customer
            {
                Id = idx,
                Name = $"Customer {idx}",
                UserRole = userRoles[idx % 2],
                Orders = new List<Order>(
                    Enumerable.Range(1, 2).Select(dx => new Order
                    {
                        Id = (idx - 1) * 2 + dx,
                        Amount = random.Next(1, 9) * 10
                    })),
                Contract = new RegularContract
                {
                    ContractId = "Id1",
                    Address = new Address
                    {
                        Id = 1,
                        City = "Lviv",
                        Country = "Ukraine",
                        Region = "Lviv"
                    }
                }
            }));

        public IQueryable<Customer> GetCustomers()
        {
            return customers.AsQueryable();
        }
    }

}
