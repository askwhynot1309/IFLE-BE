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
        public async Task<IActionResult> ViewActiveGamePackages()
        {
            var respone = await _gamePackageService.GetActiveGamePackages();
            return Ok(respone);
        }

        [HttpPost]
        [Route("{id}/games")]
        [Authorize(Roles = "Staff")]
        public async Task<IActionResult> AddGamesToPackage(List<string> gameIdList, string id)
        {
            await _gamePackageService.AddGameToPackage(id, gameIdList);
            return Ok("Thêm game vào gói thành công.");
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> ViewGamePackageDetail(string id)
        {
            var response = await _gamePackageService.GetGamePackageDetailInfo(id);
            return Ok(response);
        }

        [HttpPatch]
        [Route("{id}/inactive")]
        [Authorize(Roles = "Staff")]
        public async Task<IActionResult> SoftRemoveGamePackage(string id)
        {
            await _gamePackageService.SoftRemoveGamePackage(id);
            return Ok("Ẩn gói game thành công.");
        }

        [HttpPatch]
        [Route("{id}/active")]
        [Authorize(Roles = "Staff")]
        public async Task<IActionResult> ActivateGamePackage(string id)
        {
            await _gamePackageService.ActivateGamePackage(id);
            return Ok("Mở lại gói game thành công.");
        }

        [HttpPut]
        [Route("{id}")]
        [Authorize(Roles = "Staff")]
        public async Task<IActionResult> UpdateGamePackage(string id, GamePackageUpdateRequestModel model)
        {
            await _gamePackageService.UpdateGamePackage(model, id);
            return Ok("Cập nhật gói game thành công.");
        }
    }
}
