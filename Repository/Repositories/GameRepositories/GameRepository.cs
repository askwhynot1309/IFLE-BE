using BusinessObjects.Models;
using DAO;
using Microsoft.EntityFrameworkCore;
using Repository.Repositories.GenericRepositories;

namespace Repository.Repositories.GameRepositories
{
    public class GameRepository : GenericRepository<Game>, IGameRepository
    {
        private readonly InteractiveFloorManagementContext _context;

        public GameRepository(InteractiveFloorManagementContext context) : base(context)
        {
            _context = context;
        }

        public async Task<Game?> GetByIdWithDetailsAsync(string id)
        {
            return await _context.Games
                .Include(g => g.GameCategoryRelations)
                    .ThenInclude(gcr => gcr.GameCategory)
                .Include(g => g.GameVersions)
                .FirstOrDefaultAsync(g => g.Id == id);
        }

        public async Task<IEnumerable<Game>> GetAllWithDetailsAsync()
        {
            return await _context.Games
                .Include(g => g.GameCategoryRelations)
                    .ThenInclude(gcr => gcr.GameCategory)
                .Include(g => g.GameVersions)
                .ToListAsync();
        }

        public async Task UpdateGameCategoriesAsync(Game game, List<string> categoryIds)
        {
            // Remove existing category relations
            var existingRelations = await _context.GameCategoryRelations
                .Where(gcr => gcr.GameId == game.Id)
                .ToListAsync();
            _context.GameCategoryRelations.RemoveRange(existingRelations);

            // Add new category relations
            var newRelations = categoryIds.Select(categoryId => new GameCategoryRelation
            {
                Id = Guid.NewGuid().ToString("N").Substring(0, 32),
                GameId = game.Id,
                GameCategoryId = categoryId
            });

            await _context.GameCategoryRelations.AddRangeAsync(newRelations);
            await _context.SaveChangesAsync();
        }

        public async Task AddGameVersionAsync(Game game, GameVersion version)
        {
            version.Id = Guid.NewGuid().ToString("N").Substring(0, 32);
            version.GameId = game.Id;
            await _context.GameVersions.AddAsync(version);
            await _context.SaveChangesAsync();
        }
    }
}