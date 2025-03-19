using BLL.DTOModels;
using BLL.ServiceInterfaces;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpGet]
        public IActionResult GetProducts(string sortBy, bool ascending, string? filterName, string? filterGroupName, int filterGroupID=0, bool includeInactive=false)
        {
            var products = _productService.GetProducts(sortBy, ascending, filterName, filterGroupName, filterGroupID, includeInactive);
            return Ok(products);
        }

        [HttpPost]
        public IActionResult AddProduct(ProductRequestDTO product)
        {
            _productService.AddProduct(product);
            return Ok();
        }

        [HttpPut("deactivate/{productID}")]
        public IActionResult DeactivateProduct(int productID)
        {
            _productService.DeactivateProduct(productID);
            return Ok();
        }

        [HttpPut("activate/{productID}")]
        public IActionResult ActivateProduct(int productID)
        {
            _productService.ActivateProduct(productID);
            return Ok();
        }

        [HttpDelete("{productID}")]
        public IActionResult DeleteProduct(int productID)
        {
            _productService.DeleteProduct(productID);
            return Ok();
        }
    }

}
