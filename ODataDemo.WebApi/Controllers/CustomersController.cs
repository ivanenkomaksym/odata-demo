using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using ODataDemo.Models;
using ODataDemo.Repository;
using ODataDemo.Validators;
using ODataDemo.Visitors;

namespace ODataDemo.Controllers
{
    public class CustomersController : ODataController
    {
        private readonly ICustomerRepository _customerRepository;

        public CustomersController(ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
        }

        [QueryOptionsValidator]
        public ActionResult<IEnumerable<Customer>> Get(ODataQueryOptions<Customer> queryOptions)
        {
            var customers = _customerRepository.GetCustomers();

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
            var customers = _customerRepository.GetCustomers();

            var item = customers.SingleOrDefault(d => d.Id.Equals(key));

            if (item == null)
            {
                return NotFound();
            }

            return Ok(item);
        }
    }
}
