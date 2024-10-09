using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using ODataDemo.Models;
using System.ComponentModel.DataAnnotations;

namespace ODataDemo.Controllers
{
    [Route("odata2/{resourceId}")]
    public class OrdersController : ODataController
    {
        private static readonly int _nofOrders = 10;
        private static readonly List<Order> _orders = new(
            Enumerable.Range(1, _nofOrders).Select(idx => new Order
            {
                OrderId = $"Order {idx}",
                Amount = 2
            }));

        [HttpGet("data/{orderId}")]
        [EnableQuery]
        public ActionResult<Customer> Get([Required] string resourceId, string orderId)
        {
            var item = _orders.SingleOrDefault(d => d.OrderId.Equals(orderId));

            if (item == null)
            {
                return NotFound();
            }

            return Ok(item);
        }
    }
}
