using BusinessObjects.DTOs.OTP.Request;
using BusinessObjects.DTOs.User.Request;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Service.Services.UserServices;
using System.Security.Claims;

namespace InteractiveFloor.Controllers
{
    [Route("api/users")]
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
        [Route("{id}/info")]
        public async Task<IActionResult> UpdateUserNameInfoById(string id, [FromBody] InfoUpdateRequestModel model)
        {
            var newAccessToken = await _userService.UpdateUserOwnInformation(id, model);
            return Ok(new
            {
                message = "Cập nhật thông tin tài khoản thành công.",
                accessToken = newAccessToken
            });
        }

        [HttpPatch]
        [Authorize(Roles = "Customer")]
        [Route("{id}/avatar")]
        public async Task<IActionResult> UpdateUserAvatar(string id, [FromBody] string avatarUrl)
        {
            var newAccessToken = await _userService.UpdateUserAvatar(id, avatarUrl);
            return Ok(new
            {
                message = "Cập nhật ảnh đại diện thành công.",
                accessToken = newAccessToken
            });
        }

        [HttpPut]
        [Authorize(Roles = "Customer, Staff, Admin")]
        [Route("{id}/password")]
        public async Task<IActionResult> ChangePassword(string id, [FromBody] UserChangePasswordRequestModel model)
        {
            await _userService.ChangePassword(model, id);
            return Ok("Cập nhật mật khẩu mới thành công.");
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAllCustomers()
        {
            var response = await _userService.GetCustomerList();
            return Ok(response);
        }

        [HttpPatch]
        [Authorize(Roles = "Admin")]
        [Route("inactive")]
        public async Task<IActionResult> DeactivateCustomerAccount(List<string> userIdList)
        {
            await _userService.DeactivateCustomerAccount(userIdList);
            return Ok("Vô hiệu hóa tài khoản thành công.");
        }

        [HttpPatch]
        [Authorize(Roles = "Admin")]
        [Route("active")]
        public async Task<IActionResult> ActivateCustomerAccount(List<string> userIdList)
        {
            await _userService.ActivateCustomerAccount(userIdList);
            return Ok("Kích hoạt tài khoản thành công.");
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        [Route("staff")]
        public async Task<IActionResult> CreateStaffAccount(StaffCreateRequestModel model)
        {
            await _userService.CreateStaffAccount(model);
            return Ok("Tạo tài khoản staff mới thành công.");
        }

        [HttpGet]
        [Route("staffs")]
        public async Task<IActionResult> GetAllStaff()
        {
            var response = await _userService.GetStaffList();
            return Ok(response);
        }

        [HttpDelete]
        [Authorize(Roles = "Admin")]
        [Route("{staffId}")]
        public async Task<IActionResult> RemoveStaffAccount(string staffId)
        {
            await _userService.DeleteStaffAccount(staffId);
            return Ok("Xóa tài khoản Staff thành công.");
        }

        [HttpPatch]
        [Authorize(Roles = "Admin")]
        [Route("staffs/active")]
        public async Task<IActionResult> ActivateStaffAccount(List<string> staffIdList)
        {
            await _userService.ActivateStaffAccount(staffIdList);
            return Ok("Kích hoạt tài khoản Staff thành công.");
        }

        [HttpPatch]
        [Authorize(Roles = "Admin")]
        [Route("staffs/inactive")]
        public async Task<IActionResult> DeactivateStaffAccount(List<string> staffIdList)
        {
            await _userService.DeactivateStaffAccount(staffIdList);
            return Ok("Vô hiệu hóa tài khoản Staff thành công.");
        }

        [HttpGet]
        [Route("own-transactions")]
        public async Task<IActionResult> ViewOwnTransactions()
        {
            string currentUserId = HttpContext.User.FindFirstValue("userId");
            var response = await _userService.ViewOwnTransactions(currentUserId);
            return Ok(response);
        }
    }
}
