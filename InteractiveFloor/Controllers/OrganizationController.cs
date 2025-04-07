using BusinessObjects.DTOs.Organization.Request;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service.Services.OrganizationServices;
using System.Security.Claims;

namespace InteractiveFloor.Controllers
{
    [Route("api/organizations")]
    [ApiController]
    public class OrganizationController : Controller
    {
        private readonly IOrganizationService _organizationService;

        public OrganizationController(IOrganizationService organizationService)
        {
            _organizationService = organizationService;
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
            return Ok(await _organizationService.GetMembersOfOrganization(id));
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
            await _organizationService.UpdateOrganization(id, model);
            return Ok("Cập nhật tổ chức thành công.");
        }

        [HttpPost]
        [Route("{id}/members")]
        [Authorize(Roles = "Customer")]
        public async Task<IActionResult> AddMemberToOrganization(string id, List<string> emailList)
        {
            await _organizationService.AddUserToOrganization(emailList, id);
            return Ok("Thêm thành viên thành công.");
        }

        [HttpDelete]
        [Route("{id}/members")]
        public async Task<IActionResult> RemoveMemberOfOrganization(string id, List<string> userIdList)
        {
            await _organizationService.RemoveMemberFromOrganization(userIdList, id);
            return Ok("Xóa thành viên thành công.");
        }

        [HttpPatch]
        [Route("{id}/members/promote")]
        public async Task<IActionResult> GrantPrivilege(List<string> userIdList, string id)
        {
            await _organizationService.GrantPrivilege(userIdList, id);
            return Ok("Cấp quyền thành công.");
        }

        [HttpPatch]
        [Route("{id}/members/demote")]
        public async Task<IActionResult> RemovePrivilege(List<string> userIdList, string id)
        {
            await _organizationService.RemovePrivilege(userIdList, id);
            return Ok("Xóa quyền thành công.");
        }
    }
}
