using BusinessObjects.DTOs.GamePackage.Request;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Service.Services.GameCategoryServices;
using Service.Services.GamePackageServices;

namespace InteractiveFloor.Controllers
{
    [Route("api/game-package")]
    [ApiController]
    public class GamePackageController : ControllerBase
    {
        private readonly IGamePackageService _gamePackageService;

        public GamePackageController(IGamePackageService gamePackageService)
        {
            _gamePackageService = gamePackageService;
        }

        [HttpPost]
        [Authorize(Roles = "Staff")]
        public async Task<IActionResult> CreateGamePackage(GamePackageCreateRequestModel model)
        {
            await _gamePackageService.CreateGamePackage(model);
            return Ok("Tạo gói game thành công.");
        }

        [HttpGet]
        [Authorize(Roles = "Staff")]
        public async Task<IActionResult> ViewAllGamePackages()
        {
            var respone = await _gamePackageService.GetAllGamePackages();
            return Ok(respone);
        }

        [HttpGet]
        [Route("active")]
        [Authorize(Roles = "Staff, Customer")]
        public async Task<IActionResult> ViewActiveGamePackages()
        {
            var respone = await _gamePackageService.GetActiveGamePackages();
            return Ok(respone);
        }
    }
}
