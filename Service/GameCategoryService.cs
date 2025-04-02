using BusinessObjects.Models;
using DTO;
using Repository;
using System.Linq.Expressions;

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
            var categories = await _repository.Get();
            return categories.Select(c => new GameCategoryResponse
            {
                Id = c.Id,
                Name = c.Name,
                Description = c.Description
            }).ToList();
        }

        public async Task<GameCategoryResponse> GetByIdAsync(string id)
        {
            Expression<Func<GameCategory, bool>> filter = c => c.Id == id;
            var category = await _repository.GetSingle(filter);
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

            await _repository.Insert(category);

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

            await _repository.Update(category);

            return new GameCategoryResponse
            {
                Id = category.Id,
                Name = category.Name,
                Description = category.Description
            };
        }

        public async Task DeleteAsync(string id)
        {
            var category = new GameCategory { Id = id };
            await _repository.Delete(category);
        }
    }
}