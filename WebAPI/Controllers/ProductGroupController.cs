using BLL.DTOModels;
using BLL.ServiceInterfaces;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductGroupController : ControllerBase
    {
        private readonly IProductGroupService _productGroupService;

        public ProductGroupController(IProductGroupService productGroupService)
        {
            _productGroupService = productGroupService;
        }

        [HttpGet]
        public IActionResult GetProductGroups(int? parentID, bool ascending)
        {
            var groups = _productGroupService.GetProductGroups(parentID, ascending);
            return Ok(groups);
        }

        [HttpPost]
        public IActionResult AddProductGroup(ProductGroupRequestDTO group)
        {
            _productGroupService.AddProductGroup(group);
            return Ok();
        }
    }

}
