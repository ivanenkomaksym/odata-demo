using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using ODataDemo.Models;
using ODataDemo.Validators;
using ODataDemo.Visitors;

namespace ODataDemo.Controllers
{
    public class CustomersController : ODataController
    {
        private static readonly int nofCustomers = 10;
        private static readonly Random random = new();
        private static readonly UserRole[] userRoles = [UserRole.Admin, UserRole.User];
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
                    ContractId = "Id1"
                }
            }));

        [QueryOptionsValidator]
        public ActionResult<IEnumerable<Customer>> Get(ODataQueryOptions<Customer> queryOptions)
        {
            // Extract the filter conditions
            var filterClause = queryOptions.Filter?.FilterClause;
            if (filterClause != null)
            {
                var visitor = new FilterVisitor();
                filterClause.Expression.Accept(visitor);

                var namesToFilter = visitor.Names;

                Console.WriteLine("Filtered Names: " + string.Join(", ", namesToFilter));

                var filteredCustomers = customers.Where(x => namesToFilter.Contains(x.Name));

                return Ok(filteredCustomers);
            }

            return Ok(customers);
        }

        [EnableQuery]
        public ActionResult<Customer> Get([FromRoute] int key)
        {
            var item = customers.SingleOrDefault(d => d.Id.Equals(key));

            if (item == null)
            {
                return NotFound();
            }

            return Ok(item);
        }
    }
}
