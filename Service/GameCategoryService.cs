using BusinessObjects.Models;
using DTO;
using Repository;

namespace Service
{
    public class GameCategoryService : IGameCategoryService
    {
        private readonly IGameCategoryRepository _gameCategoryRepository;

        public GameCategoryService(IGameCategoryRepository gameCategoryRepository)
        {
            _gameCategoryRepository = gameCategoryRepository;
        }

        public async Task<List<GameCategoryResponse>> GetAllAsync()
        {
            var gameCategories = await _gameCategoryRepository.GetAllAsync();
            return gameCategories.Select(gc => new GameCategoryResponse
            {
                Id = gc.Id.Trim(),
                Name = gc.Name,
                Description = gc.Description
            }).ToList();
        }

        public async Task<GameCategoryResponse?> GetByIdAsync(string id)
        {
            var gameCategory = await _gameCategoryRepository.GetByIdAsync(id);
            if (gameCategory == null) return null;

            return new GameCategoryResponse
            {
                Id = gameCategory.Id.Trim(),
                Name = gameCategory.Name,
                Description = gameCategory.Description
            };
        }

        public async Task<GameCategoryResponse> CreateAsync(CreateGameCategoryRequest request)
        {
            var gameCategory = new GameCategory
            {
                Name = request.Name,
                Description = request.Description
            };

            var created = await _gameCategoryRepository.CreateAsync(gameCategory);
            return new GameCategoryResponse
            {
                Id = created.Id.Trim(),
                Name = created.Name,
                Description = created.Description
            };
        }

        public async Task<GameCategoryResponse> UpdateAsync(string id, UpdateGameCategoryRequest request)
        {
            var existing = await _gameCategoryRepository.GetByIdAsync(id);
            if (existing == null)
                throw new KeyNotFoundException($"GameCategory with ID {id} not found");

            existing.Name = request.Name;
            existing.Description = request.Description;

            var updated = await _gameCategoryRepository.UpdateAsync(existing);
            return new GameCategoryResponse
            {
                Id = updated.Id.Trim(),
                Name = updated.Name,
                Description = updated.Description
            };
        }

        public async Task DeleteAsync(string id)
        {
            await _gameCategoryRepository.DeleteAsync(id);
        }
    }
} 