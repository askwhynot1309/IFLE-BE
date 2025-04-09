using BusinessObjects.DTOs.UserPackage.Request;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Service.Services.UserPackageServices;

namespace InteractiveFloor.Controllers
{
    [Route("api/user-packages")]
    [ApiController]
    public class UserPackageController : ControllerBase
    {
        private readonly IUserPackageService _userPackageService;

        public UserPackageController(IUserPackageService userPackageService)
        {
            _userPackageService = userPackageService;
        }

        [HttpPost]
        [Authorize(Roles = "Staff")]
        public async Task<IActionResult> AddUserPackage(UserPackageCreateRequestModel model)
        {
            await _userPackageService.AddUserPackage(model);
            return Ok("Tạo gói người dùng thành công.");
        }

        [HttpGet]
        [Authorize(Roles = "Staff")]
        public async Task<IActionResult> ViewAllUserPackages()
        {
            var response = await _userPackageService.GetAllUserPackages();
            return Ok(response);
        }

        [HttpGet]
        [Route("active")]
        public async Task<IActionResult> ViewActiveUserPackages()
        {
            var response = await _userPackageService.GetActiveUserPackages();
            return Ok(response);
        }

        [HttpPatch]
        [Route("{id}/inactive")]
        [Authorize(Roles = "Staff")]
        public async Task<IActionResult> SoftRemoveUserPackage(string id)
        {
            await _userPackageService.SoftRemoveUserPackage(id);
            return Ok("Ẩn gói người dùng thành công.");
        }

        [HttpPatch]
        [Route("{id}/active")]
        [Authorize(Roles = "Staff")]
        public async Task<IActionResult> ActivateUserPackage(string id)
        {
            await _userPackageService.ActivateUserPackage(id);
            return Ok("Mở lại gói người dùng thành công.");
        }
    }
}
