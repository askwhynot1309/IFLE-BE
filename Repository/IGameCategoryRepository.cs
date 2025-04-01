using BusinessObjects.Models;

namespace Repository
{
    public interface IGameCategoryRepository
    {
        Task<List<GameCategory>> GetAllAsync();
        Task<GameCategory?> GetByIdAsync(string id);
        Task<GameCategory> CreateAsync(GameCategory gameCategory);
        Task<GameCategory> UpdateAsync(GameCategory gameCategory);
        Task DeleteAsync(string id);
    }
} 