using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObjects.DTOs.PlayHistory;
using BusinessObjects.Models;
using Repository.Repositories.PlayHistoryRepositories;

namespace Service.Services.PlayHistoryServices
{
    public class PlayHistoryService : IPlayHistoryService
    {
        private readonly IPlayHistoryRepository _repository;

        public PlayHistoryService(IPlayHistoryRepository repository)
        {
            _repository = repository;
        }

        public async Task<PlayHistory> CreateAsync(PlayHistoryRequest request)
        {
            var playHistory = new PlayHistory
            {
                Id = Guid.NewGuid().ToString(),
                FloorId = request.FloorId,
                UserId = request.UserId,
                Score = request.Score,
                StartAt = DateTime.Now,
                GameId = request.GameId
            };
            await _repository.Insert(playHistory);

            return playHistory;
        }

        public async Task<PlayHistory> GetHighScore(string userId, string gameId)
        {
            return await _repository.GetHighScore(userId, gameId);
        }
    }
}
