using BusinessObjects.DTOs.GameCategory;

namespace Service.Services.GameCategoryServices
{
    public interface IGameCategoryService
    {
        Task<List<GameCategoryResponse>> GetAllAsync();
        Task<GameCategoryResponse?> GetByIdAsync(string id);
        Task<GameCategoryResponse> CreateAsync(CreateGameCategoryRequest request);
        Task<GameCategoryResponse> UpdateAsync(string id, UpdateGameCategoryRequest request);
        Task DeleteAsync(string id);
    }
}