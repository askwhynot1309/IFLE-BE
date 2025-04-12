using BusinessObjects.DTOs.Device.Request;
using BusinessObjects.DTOs.InteractiveFloor.Request;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service.Services.FloorServices;
using System.Security.Claims;

namespace InteractiveFloor.Controllers
{
    [Route("api/floors")]
    [ApiController]
    public class FloorController : ControllerBase
    {
        private readonly IFloorService _floorService;

        public FloorController(IFloorService floorService)
        {
            _floorService = floorService;
        }

        [HttpPost]
        [Route("{organizationId}")]
        [Authorize(Roles = "Customer")]
        public async Task<IActionResult> CreateFloor(FloorCreateUpdateRequestModel model, string organizationId)
        {
            string currentUserId = HttpContext.User.FindFirstValue("userId");
            await _floorService.CreateFloor(model, organizationId, currentUserId);
            return Ok("Tạo sàn thành công.");
        }

        [HttpPut]
        [Route("{id}")]
        [Authorize(Roles = "Customer")]
        public async Task<IActionResult> UpdateFloor(FloorCreateUpdateRequestModel model, string id)
        {
            await _floorService.UpdateFloor(model, id);
            return Ok("Cập nhật thông tin sàn thành công.");
        }

        [HttpGet]
        [Route("{id}")]
        [Authorize(Roles = "Customer")]
        public async Task<IActionResult> ViewFloorDetails(string id)
        {
            var response = await _floorService.ViewFloorDetailInfo(id);
            return Ok(response);
        }

        [HttpPatch]
        [Route("{id}/status")]
        [Authorize(Roles = "Customer")]
        public async Task<IActionResult> SoftRemoveFloor(string id)
        {
            await _floorService.SoftRemoveFloor(id);
            return Ok("Xóa sàn tương tác thành công.");
        }

        [HttpPost]
        [Route("{id}/device")]
        [Authorize(Roles = "Customer")]
        public async Task<IActionResult> AddDeviceToFloor(string id, DeviceCreateUpdateRequestModel model)
        {
            var response = await _floorService.AddDeviceToFloor(id, model);
            return Ok(response);
        }

        [HttpGet]
        [Route("{id}/device")]
        [Authorize(Roles = "Customer")]
        public async Task<IActionResult> GetDeviceInfoOfFloor(string id)
        {
            var response = await _floorService.ViewDeviceInfoOfFloor(id);
            return Ok(response);
        }

        [HttpPut]
        [Route("{id}/device")]
        [Authorize(Roles = "Customer")]
        public async Task<IActionResult> UpdateDeviceInFloor(string id, DeviceCreateUpdateRequestModel model)
        {
            await _floorService.UpdateDeviceInFloor(id, model);
            return Ok("Cập nhật thông tin thiết bị thành công.");
        }

        [HttpDelete]
        [Route("{id}/device")]
        [Authorize(Roles = "Customer")]
        public async Task<IActionResult> RemoveDeviceFromFloor(string id)
        {
            await _floorService.RemoveDeviceFromFloor(id);
            return Ok("Xóa thiết bị thành công");
        }

        [HttpPost]
        [Route("{id}/private/member")]
        [Authorize(Roles = "Customer")]
        public async Task<IActionResult> AddMemberToPrivateFloor(string id, List<string> userIdList)
        {
            string currentUserId = HttpContext.User.FindFirstValue("userId");
            await _floorService.AddUserToPrivateFloor(userIdList, id, currentUserId);
            return Ok("Thêm người dùng vào sàn tương tác thành công.");
        }

        [HttpGet]
        [Route("{id}/private/member")]
        [Authorize(Roles = "Customer")]
        public async Task<IActionResult> GetMemberOfPrivateFloor(string id)
        {
            var response = await _floorService.GetUserInPrivateFloor(id);
            return Ok(response);
        }

        [HttpDelete]
        [Route("{id}/private/member")]
        [Authorize(Roles = "Customer")]
        public async Task<IActionResult> RemoveMemberFromPrivateFloor(string id, List<string> userIdList)
        {
            await _floorService.RemoveUserFromPrivateFloor(id, userIdList);
            return Ok("Xóa người dùng khỏi sàn tương tác riêng tư thành công.");
        }
    }
}
