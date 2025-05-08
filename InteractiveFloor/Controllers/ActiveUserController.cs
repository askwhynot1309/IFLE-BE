using Microsoft.AspNetCore.Mvc;
using Service.Services.ActiveUserServices;
using System.Threading.Tasks;

namespace InteractiveFloor.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ActiveUserController : ControllerBase
    {
        private readonly IActiveUserService _activeUserService;

        public ActiveUserController(IActiveUserService activeUserService)
        {
            _activeUserService = activeUserService;
        }

        [HttpGet]
        [Route("check-active")]
        public async Task<IActionResult> CheckActiveUser(string userId)
        {
            var activeUser = await _activeUserService.GetActiveUserById(userId);
            if (activeUser == null)
            {
                return Ok(new { isActive = false, message = "User is not active" });
            }
            
            return Ok(new { isActive = activeUser.IsActive, message = activeUser.IsActive ? "User is active" : "User is not active" });
        }
    }
} 