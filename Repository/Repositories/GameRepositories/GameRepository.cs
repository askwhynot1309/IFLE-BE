using BusinessObjects.Models;
using DAO;
using Microsoft.EntityFrameworkCore;
using Repository.Enums;
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
                Id = Guid.NewGuid().ToString(),
                GameId = game.Id,
                GameCategoryId = categoryId
            });

            await _context.GameCategoryRelations.AddRangeAsync(newRelations);
            await _context.SaveChangesAsync();
        }

        public async Task AddGameVersionAsync(Game game, GameVersion version)
        {
            version.Id = Guid.NewGuid().ToString();
            version.GameId = game.Id;
            await _context.GameVersions.AddAsync(version);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Game>> GetPurchasedGamesByUserIdAsync(string userId)
        {
            // Get all floors associated with the user
            var userFloors = await _context.PrivateFloorUsers
                .Where(fu => fu.UserId == userId && fu.InteractiveFloor.Status == "Active")
                .Select(fu => fu.FloorId)
                .ToListAsync();

            if (!userFloors.Any())
                return Enumerable.Empty<Game>();

            // Get current time for checking active orders
            var currentTime = DateTime.UtcNow;

            // Get all active game package orders for the user's floors
            var activePackageOrderIds = await _context.GamePackageOrders
                .Where(gpo => userFloors.Contains(gpo.FloorId) &&
                             gpo.Status == "Active" &&
                             gpo.EndTime > currentTime)
                .Select(gpo => gpo.GamePackageId)
                .Distinct()
                .ToListAsync();

            if (!activePackageOrderIds.Any())
                return Enumerable.Empty<Game>();

            // Get all game IDs from the active packages
            var gameIds = await _context.GamePackageRelations
                .Where(gpr => activePackageOrderIds.Contains(gpr.GamePackageId))
                .Select(gpr => gpr.GameId)
                .Distinct()
                .ToListAsync();

            if (!gameIds.Any())
                return Enumerable.Empty<Game>();

            // Get all games with their details
            return await _context.Games
                .Where(g => gameIds.Contains(g.Id) && g.Status == GameEnums.Active.ToString())
                .Include(g => g.GameCategoryRelations)
                    .ThenInclude(gcr => gcr.GameCategory)
                .Include(g => g.GameVersions)
                .ToListAsync();
        }

        public async Task<List<Game>> GetListGameByListId(List<string> gameIdList)
        {
            return (await Get(g => gameIdList.Contains(g.Id))).ToList();
        }

        public async Task<bool> IsNameExisted(string name)
        {
            return (await GetSingle(g => g.Title.ToLower().Equals(name.ToLower()))) != null;
        }
    }
}