using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Service.Services.UserServices;

namespace InteractiveFloor.Controllers
{
    [Route("api/user")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        [Authorize(Roles = "Admin, Customer")]
        [Route("{id}")]
        public async Task<IActionResult> GetUserInfoById(string id)
        {
            var result = await _userService.GetUserOwnInfo(id);
            return Ok(result);
        }
    }
}
