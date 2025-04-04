using BusinessObjects.Models;
using Repository.Repositories.GenericRepositories;

namespace Repository.Repositories.GameCategoryRepositories
{
    public interface IGameCategoryRepository : IGenericRepository<GameCategory>
    {
        Task<List<GameCategory>> GetAllAsync();
        Task<GameCategory?> GetByIdAsync(string id);
        Task<GameCategory> CreateAsync(GameCategory gameCategory);
        Task<GameCategory> UpdateAsync(GameCategory gameCategory);
        Task DeleteAsync(string id);
    }
}
