using AutoMapper;
using BusinessObjects.Models;
using DTO;
using Repository.Repositories.GameRepositories;

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
            return games.Select(g => _mapper.Map<GameResponse>(g)).ToList();
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
            var game = _mapper.Map<Game>(request);
            game.Status = "active";
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
            game.Status = "inactive";
            await _repository.Update(game);
        }
    }
}