using BLL.ServiceInterfaces;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BasketController : ControllerBase
    {
        private readonly IBasketService _basketService;

        public BasketController(IBasketService basketService)
        {
            _basketService = basketService;
        }

        [HttpPost("add")]
        public IActionResult AddToBasket(int userID, int productID, int amount)
        {
            _basketService.AddToBasket(userID, productID, amount);
            return Ok();
        }

        [HttpPut("update")]
        public IActionResult UpdateBasketItem(int userID, int productID, int amount)
        {
            _basketService.UpdateBasketItem(userID, productID, amount);
            return Ok();
        }

        [HttpDelete("remove")]
        public IActionResult RemoveFromBasket(int userID, int productID)
        {
            _basketService.RemoveFromBasket(userID, productID);
            return Ok();
        }

        [HttpPost("generate-order")]
        public IActionResult GenerateOrder(int userID)
        {
            var orderId = _basketService.GenerateOrder(userID);
            return Ok(new { OrderID = orderId });
        }
    }

}
