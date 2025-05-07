using BusinessObjects.DTOs.GameLog;
using BusinessObjects.Models;
using Repository.Repositories.GameLogRepositories;
using Repository.Repositories.GameRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Service.Services.GameLogServices
{
    public class GameLogService : IGameLogService
    {
        private readonly IGameLogRepository _gameLogRepository;
        private readonly IGameRepository _gameRepository;

        public GameLogService(IGameLogRepository gameLogRepository, IGameRepository gameRepository)
        {
            _gameLogRepository = gameLogRepository;
            _gameRepository = gameRepository;
        }

        public async Task<GameLogResponse> CreateGameLogAsync(CreateGameLog createGameLog)
        {
            var game = await _gameRepository.GetSingle(g => g.Id == createGameLog.GameId);
            if (game == null)
                throw new Exception("Game not found");

            var gameLog = new GameLog
            {
                Id = Guid.NewGuid().ToString(),
                Description = createGameLog.Description,
                StaffId = createGameLog.StaffId,
                GameId = createGameLog.GameId,
                UpdateStatus = game.Status,
                UpdateTime = DateTime.Now,
            };

            await _gameLogRepository.Insert(gameLog);

            var staff = await _gameLogRepository.GetSingle(g => g.Id == gameLog.Id, includeProperties: "Staff,Game");
            return new GameLogResponse
            {
                Id = gameLog.Id,
                UpdateTime = gameLog.UpdateTime,
                Description = gameLog.Description,
                StaffId = gameLog.StaffId,
                GameId = gameLog.GameId,
                StaffName = staff?.Staff?.FullName ?? "Unknown",
                GameTitle = staff?.Game?.Title ?? "Unknown"
            };
        }

        public async Task<IEnumerable<GameLogResponse>> GetAllGameLogsAsync()
        {
            var gameLogs = await _gameLogRepository.Get(includeProperties: "Staff,Game");
            return gameLogs.Select(g => new GameLogResponse
            {
                Id = g.Id,
                UpdateTime = g.UpdateTime,
                Description = g.Description,
                StaffId = g.StaffId,
                GameId = g.GameId,
                StaffName = g.Staff?.FullName ?? "Unknown",
                GameTitle = g.Game?.Title ?? "Unknown"
            });
        }

        public async Task<IEnumerable<GameLogResponse>> GetGameLogsByGameIdAsync(string gameId)
        {
            var gameLogs = await _gameLogRepository.Get(
                filter: g => g.GameId == gameId,
                includeProperties: "Staff,Game"
            );
            return gameLogs.Select(g => new GameLogResponse
            {
                Id = g.Id,
                UpdateTime = g.UpdateTime,
                Description = g.Description,
                StaffId = g.StaffId,
                GameId = g.GameId,
                StaffName = g.Staff?.FullName ?? "Unknown",
                GameTitle = g.Game?.Title ?? "Unknown"
            });
        }
    }
}