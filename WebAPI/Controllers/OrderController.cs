using BLL.ServiceInterfaces;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpGet("{orderID}")]
        public IActionResult GetOrderDetails(int orderID)
        {
            var orderDetails = _orderService.GetOrderDetails(orderID);
            return Ok(orderDetails);
        }

        [HttpGet]
        public IActionResult GetOrders(string sortBy, bool ascending, int filterID, bool filterPaid)
        {
            var orders = _orderService.GetOrders(sortBy, ascending, filterID, filterPaid);
            return Ok(orders);
        }

        [HttpPost("pay")]
        public IActionResult PayOrder(int orderID, double amount)
        {
            try
            {
                _orderService.PayOrder(orderID, amount);
                return Ok();
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }

}
