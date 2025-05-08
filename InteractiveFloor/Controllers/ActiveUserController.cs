using BusinessObjects.Models;
using Microsoft.AspNetCore.Mvc;
using Service.Services.ActiveUserServices;
using System.Threading.Tasks;

namespace InteractiveFloor.Controllers
{
    [ApiController]
    [Route("api/active-user")]
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

        [HttpPost]
        public async Task<IActionResult> InsertActiveUser(string userId)
        {

            await _activeUserService.TrackUserLogin(userId);
          
            return Ok();
        } 

        [HttpPost]
        [Route("logout")]
        public async Task<IActionResult> InactiveUser(string userId)
        {
            await _activeUserService.UpdateUserActiveStatus(userId, false);
            return Ok();
        }

    }
} 