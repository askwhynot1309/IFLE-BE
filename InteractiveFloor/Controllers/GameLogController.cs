using BusinessObjects.DTOs.GameLog;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service.Services;
using Service.Services.GameLogServices;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace InteractiveFloor.Controllers
{
    [Route("api/game-log")]
    [ApiController]
    public class GameLogController : ControllerBase
    {
        private readonly IGameLogService _gameLogService;

        public GameLogController(IGameLogService gameLogService)
        {
            _gameLogService = gameLogService;
        }

        [HttpPost]
        public async Task<ActionResult<GameLogResponse>> CreateGameLog([FromBody] CreateGameLog createGameLog)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var result = await _gameLogService.CreateGameLogAsync(createGameLog);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    message = ex.Message
                });
            }
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<GameLogResponse>>> GetAllGameLogs()
        {
            try
            {
                var result = await _gameLogService.GetAllGameLogsAsync();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    message = ex.Message
                });
            }
        }

        [HttpGet("game/{gameId}")]
        public async Task<ActionResult<IEnumerable<GameLogResponse>>> GetGameLogsByGameId(string gameId)
        {
            try
            {
                if (string.IsNullOrEmpty(gameId))
                {
                    return BadRequest(new
                    {
                        message = "Game ID is required"
                    });
                }

                var result = await _gameLogService.GetGameLogsByGameIdAsync(gameId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    message = ex.Message
                });
            }
        }
    }
} 