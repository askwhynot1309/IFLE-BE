using BusinessObjects.DTOs.GameCategory;
using BusinessObjects.Models;
using Repository.Repositories.GameCategoryRelationRepositories;
using Repository.Repositories.GameCategoryRepositories;
using System.Linq.Expressions;

namespace Service.Services.GameCategoryServices
{
    public class GameCategoryService : IGameCategoryService
    {
        private readonly IGameCategoryRepository _repository;
        private readonly IGameCategoryRelationRepository _gameCategoryRelationRepository;

        public GameCategoryService(IGameCategoryRepository repository, IGameCategoryRelationRepository gameCategoryRelationRepository)
        {
            _repository = repository;
            _gameCategoryRelationRepository = gameCategoryRelationRepository;
        }

        public async Task<List<GameCategoryResponse>> GetAllAsync()
        {
            var categories = await _repository.Get();
            return categories.Select(c => new GameCategoryResponse
            {
                Id = c.Id,
                Name = c.Name,
                Description = c.Description
            }).OrderBy(c => c.Name).ToList();
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
                Id = Guid.NewGuid().ToString(),
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
            var relations = await _gameCategoryRelationRepository.GetListByGameCategory(id);

            await _gameCategoryRelationRepository.DeleteRange(relations);
            await _repository.Delete(category);
        }
    }
}