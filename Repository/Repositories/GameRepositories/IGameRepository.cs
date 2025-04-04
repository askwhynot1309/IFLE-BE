using BusinessObjects.Models;
using Repository.Repositories.GenericRepositories;

namespace Repository.Repositories.GameRepositories
{
    public interface IGameRepository : IGenericRepository<Game>
    {
        Task<Game?> GetByIdWithDetailsAsync(string id);
        Task<IEnumerable<Game>> GetAllWithDetailsAsync();
        Task UpdateGameCategoriesAsync(Game game, List<string> categoryIds);
        Task AddGameVersionAsync(Game game, GameVersion version);
    }
}