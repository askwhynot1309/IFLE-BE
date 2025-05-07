using AutoMapper;
using BusinessObjects.DTOs.Game;
using BusinessObjects.Models;
using Repository.Enums;
using Repository.Repositories.GamePackageRelationRepositories;
using Repository.Repositories.GamePackageRepositories;
using Repository.Repositories.GameRepositories;
using Service.Ultis;

namespace Service.Services.GameServices
{
    public class GameService : IGameService
    {
        private readonly IGameRepository _repository;
        private readonly IMapper _mapper;

        public GameService(IGameRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<List<GameResponse>> GetAllAsync()
        {
            var games = await _repository.GetAllWithDetailsAsync();
            return games.Select(g => _mapper.Map<GameResponse>(g)).OrderBy(g => g.Title).ToList();
        }

        public async Task<GameResponse?> GetByIdAsync(string id)
        {
            var game = await _repository.GetByIdWithDetailsAsync(id);
            if (game == null)
                return null;

            return _mapper.Map<GameResponse>(game);
        }

        public async Task<GameResponse> CreateAsync(CreateGameRequest request)
        {
            var check = await _repository.IsNameExisted(request.Title);

            if (check)
            {
                throw new CustomException("Tên trò chơi này đã tồn tại.");
            }
            var game = _mapper.Map<Game>(request);
            game.Status = GameEnums.Active.ToString();
            game.PlayCount = 0;
            await _repository.Insert(game);

            // Add categories
            await _repository.UpdateGameCategoriesAsync(game, request.CategoryIds);

            // Add initial version if provided
            if (request.Version != null)
            {
                var version = _mapper.Map<GameVersion>(request.Version);
                await _repository.AddGameVersionAsync(game, version);
            }

            // Get the complete game with relations
            var createdGame = await _repository.GetByIdWithDetailsAsync(game.Id);
            return _mapper.Map<GameResponse>(createdGame);
        }

        public async Task<GameResponse> UpdateAsync(string id, UpdateGameRequest request)
        {
            var existingGame = await _repository.GetByIdWithDetailsAsync(id);
            if (existingGame == null)
                throw new KeyNotFoundException($"Game with ID {id} not found.");

            if (!existingGame.Title.ToLower().Equals(request.Title.ToLower()))
            {
                var check = await _repository.IsNameExisted(request.Title);

                if (check)
                {
                    throw new CustomException("Tên trò chơi này đã tồn tại.");
                }
            }
            // Update basic properties
            _mapper.Map(request, existingGame);
            await _repository.Update(existingGame);

            // Update categories
            await _repository.UpdateGameCategoriesAsync(existingGame, request.CategoryIds);

            // Get the updated game with relations
            var updatedGame = await _repository.GetByIdWithDetailsAsync(id);
            return _mapper.Map<GameResponse>(updatedGame);
        }

        public async Task DeleteAsync(string id)
        {
            var game = await _repository.GetByIdWithDetailsAsync(id);
            if (game == null)
                throw new KeyNotFoundException($"Game with ID {id} not found.");
            game.Status = GameEnums.Inactive.ToString();
            await _repository.Update(game);
        }

        public async Task UpdatePlayCount(string id)
        {
            var game = await _repository.GetByIdWithDetailsAsync(id);
            if (game == null)
                throw new KeyNotFoundException($"Game with ID {id} not found.");
            //update game coutn by 1
            game.PlayCount += 1;
            await _repository.Update(game);
        }

        public async Task<string> GetDownloadUrl(string id)
        {
            var game = await _repository.GetByIdWithDetailsAsync(id);
            if (game == null)
                throw new KeyNotFoundException($"Game with ID {id} not found.");
            return game.DownloadUrl;
        }

        public async Task<GameResponse> AddVersionAsync(string gameId, AddGameVersionRequest request)
        {
            var game = await _repository.GetByIdWithDetailsAsync(gameId);
            if (game == null)
                throw new Exception("Game not found");

            // Create new version
            var version = _mapper.Map<GameVersion>(request);
            await _repository.AddGameVersionAsync(game, version);

            // Update game's download URL
            game.DownloadUrl = request.DownloadUrl;
            await _repository.Update(game);

            // Get updated game with relations
            var updatedGame = await _repository.GetByIdWithDetailsAsync(gameId);
            return _mapper.Map<GameResponse>(updatedGame);
        }

        public async Task<List<GameResponse>> GetPurchasedGamesByUserIdAsync(string userId)
        {
            var games = await _repository.GetPurchasedGamesByUserIdAsync(userId);
            return games.Select(g => _mapper.Map<GameResponse>(g)).ToList();
        }
    }
}