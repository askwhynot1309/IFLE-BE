using BusinessObjects.DTOs.GamePackageOrder.Request;
using BusinessObjects.DTOs.Organization.Request;
using BusinessObjects.DTOs.UserPackageOrder.Request;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service.Services.OrganizationServices;
using Service.Services.PayosServices;
using System.Security.Claims;

namespace InteractiveFloor.Controllers
{
    [Route("api/organizations")]
    [ApiController]
    public class OrganizationController : Controller
    {
        private readonly IOrganizationService _organizationService;
        private readonly IPayosService _payosService;

        public OrganizationController(IOrganizationService organizationService, IPayosService payosService)
        {
            _organizationService = organizationService;
            _payosService = payosService;
        }

        [HttpPost]
        [Authorize(Roles = "Customer")]
        public async Task<IActionResult> CreateOrganization(OrganizationCreateUpdateRequestModel model)
        {
            string currentUserId = HttpContext.User.FindFirstValue("userId");
            await _organizationService.CreateOrganization(model, currentUserId);
            return Ok("Tạo tổ chức thành công.");
        }

        [HttpGet]
        [Authorize(Roles = "Admin, Staff")]
        public async Task<IActionResult> ViewOrganization()
        {
            var response = await _organizationService.ViewAllOrganizations();
            return Ok(response);
        }

        [HttpGet]
        [Route("{id}/members")]
        [Authorize(Roles = "Customer")]
        public async Task<IActionResult> GetMembersByOrganizationId(string id)
        {
            string currentUserId = HttpContext.User.FindFirstValue("userId");
            return Ok(await _organizationService.GetMembersOfOrganization(id, currentUserId));
        }

        [HttpGet]
        [Route("own")]
        [Authorize(Roles = "Customer")]
        public async Task<IActionResult> GetOwnOrganization()
        {
            string currentUserId = HttpContext.User.FindFirstValue("userId");
            var response = await _organizationService.GetOwnOrganization(currentUserId);
            return Ok(response);
        }

        [HttpPut]
        [Route("{id}")]
        [Authorize(Roles = "Customer")]
        public async Task<IActionResult> UpdateOrganization(string id, OrganizationCreateUpdateRequestModel model)
        {
            string currentUserId = HttpContext.User.FindFirstValue("userId");
            await _organizationService.UpdateOrganization(id, model, currentUserId);
            return Ok("Cập nhật tổ chức thành công.");
        }

        [HttpPatch]
        [Route("{id}/status")]
        [Authorize(Roles = "Customer")]
        public async Task<IActionResult> SoftRemoveOrganization(string id)
        {
            string currentUserId = HttpContext.User.FindFirstValue("userId");
            await _organizationService.SoftRemoveOrganization(id, currentUserId);
            return Ok("Xóa tổ chức thành công.");
        }

        [HttpPost]
        [Route("{id}/members")]
        [Authorize(Roles = "Customer")]
        public async Task<IActionResult> AddMemberToOrganization(string id, List<string> emailList)
        {
            string currentUserId = HttpContext.User.FindFirstValue("userId");
            await _organizationService.AddUserToOrganization(emailList, id, currentUserId);
            return Ok("Thêm thành viên thành công.");
        }

        [HttpDelete]
        [Route("{id}/members")]
        [Authorize(Roles = "Customer")]
        public async Task<IActionResult> RemoveMemberOfOrganization(string id, List<string> userIdList)
        {
            string currentUserId = HttpContext.User.FindFirstValue("userId");
            await _organizationService.RemoveMemberFromOrganization(userIdList, id, currentUserId);
            return Ok("Xóa thành viên thành công.");
        }

        [HttpPatch]
        [Route("{id}/members/promote")]
        [Authorize(Roles = "Customer")]
        public async Task<IActionResult> GrantPrivilege(List<string> userIdList, string id)
        {
            string currentUserId = HttpContext.User.FindFirstValue("userId");
            await _organizationService.GrantPrivilege(userIdList, id, currentUserId);
            return Ok("Cấp quyền thành công.");
        }

        [HttpPatch]
        [Route("{id}/members/demote")]
        [Authorize(Roles = "Customer")]
        public async Task<IActionResult> RemovePrivilege(List<string> userIdList, string id)
        {
            string currentUserId = HttpContext.User.FindFirstValue("userId");
            await _organizationService.RemovePrivilege(userIdList, id, currentUserId);
            return Ok("Xóa quyền thành công.");
        }

        [HttpGet]
        [Route("{id}")]
        [Authorize(Roles = "Customer")]
        public async Task<IActionResult> ViewDetailOfOrganization(string id)
        {
            string currentUserId = HttpContext.User.FindFirstValue("userId");
            var response = await _organizationService.GetDetailsInfoOfOrganization(id, currentUserId);
            return Ok(response);
        }

        [HttpGet]
        [Route("{id}/floors")]
        public async Task<IActionResult> GetFloorsOfOrganization(string id)
        {
            string currentUserId = HttpContext.User.FindFirstValue("userId");
            var response = await _organizationService.GetFloorListOfOrganization(id, currentUserId);
            return Ok(response);
        }

        [HttpPost]
        [Route("{id}/user-package")]
        [Authorize(Roles = "Customer")]
        public async Task<IActionResult> BuyUserPackageForFloor(string id, UserPackageOrderCreateRequestModel model)
        {
            string currentUserId = HttpContext.User.FindFirstValue("userId");
            var response = await _organizationService.BuyUserPackageForOrganization(id, model, currentUserId);
            return Ok(response);
        }

        [HttpPatch]
        [Route("user-package/status")]
        [Authorize(Roles = "Customer")]
        public async Task<IActionResult> UpdateUserPackageOrderStatus(string orderCode)
        {
            string currentUserId = HttpContext.User.FindFirstValue("userId");

            await _organizationService.UpdateUserPackageOrderStatus(orderCode, currentUserId);

            return Ok("Cập nhật trạng thái thanh toán thành công.");
        }

        [HttpGet]
        [Route("{id}/transactions")]
        [Authorize(Roles = "Customer,Admin")]
        public async Task<IActionResult> GetAllUserPackageOrderOfOrganization(string id)
        {
            var response = await _organizationService.GetAllUserPackageOrders(id);
            return Ok(response);
        }

        [HttpGet]
        [Route("user-package/{orderId}/payment-link")]
        [Authorize(Roles = "Customer")]
        public async Task<IActionResult> GetContinuePaymentLinkForUserPackageOrder(string orderId)
        {
            var response = await _organizationService.CreateAgainPaymentUrlForPendingUserPackageOrder(orderId);
            return Ok(response);
        }

        [HttpGet]
        [Route("user-package/{orderId}")]
        public async Task<IActionResult> GetUserPackageOrderDetails(string orderId)
        {
            var response = await _organizationService.GetUserPackageOrderDetails(orderId);
            return Ok(response);
        }
    }
}
