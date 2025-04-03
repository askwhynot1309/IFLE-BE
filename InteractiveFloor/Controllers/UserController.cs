using BusinessObjects.DTOs.OTP.Request;
using BusinessObjects.DTOs.User.Request;
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

        [HttpPut]
        [Authorize(Roles = "Customer")]
        [Route("{id}")]
        public async Task<IActionResult> UpdateUserOwnInfoById(string id, [FromBody] UserUpdateRequestModel model)
        {
            await _userService.UpdateUserOwnInformation(id, model);
            return Ok("Cập nhật thông tin tài khoản thành công.");
        }

        [HttpPut]
        [Authorize(Roles = "Customer, Staff, Admin")]
        [Route("{id}/password")]
        public async Task<IActionResult> ChangePassword(string id, [FromBody] UserChangePasswordRequestModel model)
        {
            await _userService.ChangePassword(model, id);
            return Ok("Cập nhật mật khẩu mới thành công.");
        }
    }
}
