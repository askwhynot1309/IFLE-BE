using DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service.Services.GameServices;

namespace InteractiveFloor.Controllers
{
    [ApiController]
    [Route("api/game")]
    public class GameController : ControllerBase
    {
        private readonly IGameService _gameService;
        private static readonly HttpClient httpClient = new HttpClient();

        public GameController(IGameService gameService)
        {
            _gameService = gameService;
        }

        [HttpGet]
        [Authorize(Roles = "Staff, Customer")]
        public async Task<ActionResult<List<GameResponse>>> GetAll()
        {
            try
            {
                var games = await _gameService.GetAllAsync();
                return Ok(games);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while retrieving games.", error = ex.Message });
            }
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "Staff, Customer")]
        public async Task<ActionResult<GameResponse>> GetById(string id)
        {
            try
            {
                var game = await _gameService.GetByIdAsync(id);
                if (game == null)
                    return NotFound(new { message = $"Game with ID {id} not found." });

                return Ok(game);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while retrieving the game.", error = ex.Message });
            }
        }

        [HttpPost]
        [Authorize(Roles = "Staff")]
        public async Task<ActionResult<GameResponse>> Create(CreateGameRequest request)
        {
            try
            {
                var game = await _gameService.CreateAsync(request);
                return CreatedAtAction(nameof(GetById), new { id = game.Id }, game);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while creating the game.", error = ex.Message });
            }
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Staff")]
        public async Task<ActionResult<GameResponse>> Update(string id, UpdateGameRequest request)
        {
            try
            {
                var game = await _gameService.UpdateAsync(id, request);
                return Ok(game);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while updating the game.", error = ex.Message });
            }
        }

        [HttpPut("disable-game/{id}")]
        [Authorize(Roles = "Staff")]
        public async Task<IActionResult> DisableGame(string id)
        {
            try
            {
                await _gameService.DeleteAsync(id);
                return Ok("Disabled");
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while deleting the game.", error = ex.Message });
            }
        }

        [HttpPut("update-game-count/{id}")]
        [Authorize(Roles = "Customer")]
        public async Task<IActionResult> UpdateGameCount(string id)
        {
            try
            {
                await _gameService.UpdatePlayCount(id);
                return Ok("Game count increased by 1");
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while deleting the game.", error = ex.Message });
            }
        }

        [HttpPost("{id}/versions")]
        [Authorize(Roles = "Staff")]
        public async Task<ActionResult<GameResponse>> AddVersion(string id, AddGameVersionRequest request)
        {
            try
            {
                var result = await _gameService.AddVersionAsync(id, request);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("user/{userId}/purchased")]
        [Authorize(Roles = "Staff, Customer")]
        public async Task<ActionResult<List<GameResponse>>> GetPurchasedGames(string userId)
        {
            try
            {
                var result = await _gameService.GetPurchasedGamesByUserIdAsync(userId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        //[HttpGet("download/{id}")]
        //public async Task<IActionResult> DownloadGame(string id)
        //{
        //    var downloadUrl = await _gameService.GetDownloadUrl(id); 

        //    using (var httpClient = new HttpClient())
        //    {
        //        var response = await httpClient.GetAsync(downloadUrl);
        //        if (!response.IsSuccessStatusCode)
        //        {
        //            return NotFound("File not found at download URL.");
        //        }

        //        var fileBytes = await response.Content.ReadAsByteArrayAsync();
        //        var contentType = "application/zip";
        //        var fileName = "game.zip";

        //        return File(fileBytes, contentType, fileName);
        //    }
        //}

        [HttpGet("download/game-launcher")]
        [Authorize(Roles = "Customer")]
        public async Task<IActionResult> DownloadGameLauncher()
        {
            var downloadUrl = "https://github.com/askwhynot1309/IFLE-Game-Launcher/releases/download/v1.0/IFLE-Launcher.zip";

            var response = await httpClient.GetAsync(downloadUrl, HttpCompletionOption.ResponseHeadersRead);
            if (!response.IsSuccessStatusCode)
            {
                return NotFound("File not found at download URL.");
            }

            var stream = await response.Content.ReadAsStreamAsync();
            var contentType = "application/zip";
            var fileName = "IFLE-launcher.zip";

            return File(stream, contentType, fileName);
        }


    }
} 