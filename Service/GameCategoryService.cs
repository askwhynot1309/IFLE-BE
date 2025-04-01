using BusinessObjects.Models;
using DTO;
using Repository;

namespace Service
{
    public class GameCategoryService : IGameCategoryService
    {
        private readonly IGameCategoryRepository _repository;

        public GameCategoryService(IGameCategoryRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<GameCategoryResponse>> GetAllAsync()
        {
            var categories = await _repository.GetAllAsync();
            return categories.Select(c => new GameCategoryResponse
            {
                Id = c.Id,
                Name = c.Name,
                Description = c.Description
            }).ToList();
        }

        public async Task<GameCategoryResponse> GetByIdAsync(string id)
        {
            var category = await _repository.GetByIdAsync(id);
            if (category == null)
                return null;

            return new GameCategoryResponse
            {
                Id = category.Id,
                Name = category.Name,
                Description = category.Description
            };
        }

        public async Task<GameCategoryResponse> CreateAsync(CreateGameCategoryRequest request)
        {
            var category = new GameCategory
            {
                Id = Guid.NewGuid().ToString("N").Substring(0, 32),
                Name = request.Name,
                Description = request.Description
            };

            await _repository.CreateAsync(category);

            return new GameCategoryResponse
            {
                Id = category.Id,
                Name = category.Name,
                Description = category.Description
            };
        }

        public async Task<GameCategoryResponse> UpdateAsync(string id, UpdateGameCategoryRequest request)
        {
            var category = new GameCategory
            {
                Id = id,
                Name = request.Name,
                Description = request.Description
            };

            await _repository.UpdateAsync(category);

            return new GameCategoryResponse
            {
                Id = category.Id,
                Name = category.Name,
                Description = category.Description
            };
        }

        public async Task DeleteAsync(string id)
        {
            await _repository.DeleteAsync(id);
        }
    }
} 