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
        public async Task<IActionResult> CreateOrganization(OrganizationCreateRequestModel model)
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
        public async Task<IActionResult> GetMembersByOrganizationId(string id)
        {
            return Ok(await _organizationService.GetMembersOfOrganization(id));
        }

        [HttpGet]
        [Route("own")]
        public async Task<IActionResult> GetOwnOrganization()
        {
            string currentUserId = HttpContext.User.FindFirstValue("userId");
            var response = await _organizationService.GetOwnOrganization(currentUserId);
            return Ok(response);
        }
    }
}
