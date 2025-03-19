using BLL.DTOModels;
using BLL.ServiceInterfaces;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("login")]
        public IActionResult Login(UserLoginRequestDTO loginRequest)
        {
            var response = _userService.Login(loginRequest);
            if (response != null)
                return Ok(response);
            return Unauthorized();
        }

        [HttpPost("logout/{userID}")]
        public IActionResult Logout(int userID)
        {
            _userService.Logout(userID);
            return Ok();
        }
    }

}
