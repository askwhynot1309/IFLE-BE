using BusinessObjects.DTOs.Device.Request;
using BusinessObjects.DTOs.GamePackageOrder.Request;
using BusinessObjects.DTOs.InteractiveFloor.Request;
using BusinessObjects.DTOs.SetUpGuide.Request;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service.Services.FloorServices;
using Service.Services.PayosServices;
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

        [HttpGet]
        [Authorize(Roles = "Staff,Admin")]
        public async Task<IActionResult> GetAllFloors()
        {
            var response = await _floorService.GetAllFloors();
            return Ok(response);
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
            string currentUserId = HttpContext.User.FindFirstValue("userId");
            await _floorService.UpdateFloor(model, id, currentUserId);
            return Ok("Cập nhật thông tin sàn thành công.");
        }

        [HttpGet]
        [Route("{id}")]
        [Authorize(Roles = "Customer")]
        public async Task<IActionResult> ViewFloorDetails(string id)
        {
            string currentUserId = HttpContext.User.FindFirstValue("userId");
            var response = await _floorService.ViewFloorDetailInfo(id, currentUserId);
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

        [HttpPost]
        [Route("{id}/game-package")]
        [Authorize(Roles = "Customer")]
        public async Task<IActionResult> BuyGamePackageForFloor(string id, GamePackageOrderCreateRequestModel model)
        {
            string currentUserId = HttpContext.User.FindFirstValue("userId");
            var response = await _floorService.BuyGamePackageForFloor(id, model, currentUserId);
            return Ok(response);
        }

        [HttpPatch]
        [Route("game-package/status")]
        [Authorize(Roles = "Customer")]
        public async Task<IActionResult> UpdateGamePackageOrderStatus(string orderCode)
        {
            string currentUserId = HttpContext.User.FindFirstValue("userId");
            await _floorService.UpdateGamePackageOrderStatus(orderCode, currentUserId);

            return Ok("Cập nhật trạng thái thanh toán thành công.");
        }

        [HttpGet]
        [Route("{id}/game-package/available")]
        [Authorize(Roles = "Customer")]
        public async Task<IActionResult> GetAvailableGameForFloor(string id)
        {
            var response = await _floorService.GetAllAvailableGamePackageOfFloor(id);
            return Ok(response);
        }

        [HttpGet]
        [Route("{id}/game-package/playable")]
        [Authorize(Roles = "Customer")]
        public async Task<IActionResult> GetPlayableGameForFloor(string id)
        {
            var response = await _floorService.GetPlayableGamePackageOfFloor(id);
            return Ok(response);
        }

        [HttpPatch]
        [Route("game-package/order/activate")]
        [Authorize(Roles = "Customer")]
        public async Task<IActionResult> ActivateGamePackageAfterBuying([FromBody] string gamePackageOrderId)
        {
            await _floorService.ActivateGamePackageOrder(gamePackageOrderId);
            return Ok("Kích hoạt gói game thành công.");
        }

        [HttpGet]
        [Route("{id}/transactions")]
        [Authorize(Roles = "Customer,Admin")]
        public async Task<IActionResult> GetAllGamePackageOrderOfFloor(string id)
        {
            var response = await _floorService.GetGamePackageOrderOfFloor(id);
            return Ok(response);
        }

        [HttpGet]
        [Route("game-package/{orderId}/payment-link")]
        public async Task<IActionResult> GetContinuePaymentLinkForGamePackageOrder(string orderId)
        {
            var response = await _floorService.CreateAgainPaymentUrlForPendingGamePackageOrder(orderId);
            return Ok(response);
        }

        [HttpGet]
        [Route("game-package/{orderId}")]
        [Authorize(Roles = "Customer")]
        public async Task<IActionResult> GetGamePackageOrderDetails(string orderId)
        {
            var response = await _floorService.GetGamePackageOrderDetails(orderId);
            return Ok(response);
        }

        [HttpGet]
        [Route("{id}/setup-guide")]
        [Authorize(Roles = "Customer")]
        public async Task<IActionResult> GetPlayFloorSetUpGuide(string id, [FromQuery] SetUpGuideRequestModel model)
        {
            var response = await _floorService.GetSetUpGuideForCustomer(model, id);
            return Ok(response);
        }
    }
}
