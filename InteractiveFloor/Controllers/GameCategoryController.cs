using BusinessObjects.DTOs.GameCategory;
using Microsoft.AspNetCore.Mvc;
using Service.Services.GameCategoryServices;

namespace InteractiveFloor.Controllers
{
    [ApiController]
    [Route("api/game-category")]
    public class GameCategoryController : ControllerBase
    {
        private readonly IGameCategoryService _gameCategoryService;

        public GameCategoryController(IGameCategoryService gameCategoryService)
        {
            _gameCategoryService = gameCategoryService;
        }

        [HttpGet]
        public async Task<ActionResult<List<GameCategoryResponse>>> GetAll()
        {
            var gameCategories = await _gameCategoryService.GetAllAsync();
            return Ok(gameCategories);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<GameCategoryResponse>> GetById(string id)
        {
            var gameCategory = await _gameCategoryService.GetByIdAsync(id);
            if (gameCategory == null)
                return NotFound();

            return Ok(gameCategory);
        }

        [HttpPost]
        public async Task<ActionResult<GameCategoryResponse>> Create(CreateGameCategoryRequest request)
        {
            var gameCategory = await _gameCategoryService.CreateAsync(request);
            return CreatedAtAction(nameof(GetById), new { id = gameCategory.Id }, gameCategory);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<GameCategoryResponse>> Update(string id, UpdateGameCategoryRequest request)
        {
            try
            {
                var gameCategory = await _gameCategoryService.UpdateAsync(id, request);
                return Ok(gameCategory);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            await _gameCategoryService.DeleteAsync(id);
            return NoContent();
        }
    }
}