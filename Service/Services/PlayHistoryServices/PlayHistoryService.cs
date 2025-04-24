using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using BusinessObjects.DTOs.PlayHistory;
using BusinessObjects.Models;
using Repository.Repositories.PlayHistoryRepositories;

namespace Service.Services.PlayHistoryServices
{
    public class PlayHistoryService : IPlayHistoryService
    {
        private readonly IPlayHistoryRepository _repository;
        private readonly IMapper _mapper;

        public PlayHistoryService(IPlayHistoryRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
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

        public async Task<List<PlayHistoryFloorResponse>> GetFloorPlayHistory(string floorId)
        {
            var floorPlayHistory = await _repository.GetFloorPlayHistory(floorId);

            return _mapper.Map<List<PlayHistoryFloorResponse>>(floorPlayHistory);
        }

        public async Task<PlayHistoryResponse> GetHighScore(string userId, string gameId)
        {
            var highscore = await _repository.GetHighScore(userId, gameId);
            if (highscore == null)
                return null;
            return _mapper.Map<PlayHistoryResponse>(highscore);
        }
    }
}
