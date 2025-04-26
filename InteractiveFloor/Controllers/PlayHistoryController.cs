using BusinessObjects.Models;
using Microsoft.AspNetCore.Mvc;
using Service.Services.PlayHistoryServices;
using Microsoft.AspNetCore.Authorization;
using BusinessObjects.DTOs.PlayHistory;

namespace InteractiveFloor.Controllers
{
    [ApiController]
    [Route("api")]
    public class PlayHistoryController: ControllerBase
    {
        private readonly IPlayHistoryService _playHistoryService;
        public PlayHistoryController(IPlayHistoryService playHistoryService)
        {
            _playHistoryService = playHistoryService;
        }

        [HttpGet("history/get-high-score")]
        [Authorize(Roles = "Customer")]
        public async Task<ActionResult<PlayHistoryResponse>> GetHighScore(string userId, string gameId)
        {
            try
            {
                var scoreO = await _playHistoryService.GetHighScore(userId, gameId);
                if (scoreO == null)
                    return NotFound(new { message = $"Game with ID {gameId} and {userId} not found." });

                return Ok(scoreO);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while retrieving the highscore.", error = ex.Message });
            }
        }

        [HttpPost("history/new-play-history")]
        [Authorize(Roles = "Customer")]
        public async Task<ActionResult<PlayHistory>> CreateHighScore(PlayHistoryRequest request)
        {
            try
            {
                var game = await _playHistoryService.CreateAsync(request);
                return Ok(game);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while creating the score record.", error = ex.Message });
            }
        }

        [HttpGet("history/floor-play-history/{floorId}")]
        [Authorize(Roles = "Staff, Customer")]
        public async Task<ActionResult<List<PlayHistory>>> GetFloorPlayHistory(string floorId)
        {
            try
            {
                var floorPlayHistory = await _playHistoryService.GetFloorPlayHistory(floorId);
                return Ok(floorPlayHistory);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while getting the play history for the floor.", error = ex.Message });
            }
        }
    }
}
