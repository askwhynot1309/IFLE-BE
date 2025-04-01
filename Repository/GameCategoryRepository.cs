using BusinessObjects.Models;
using DAO;
using Microsoft.EntityFrameworkCore;

namespace Repository
{
    public class GameCategoryRepository : IGameCategoryRepository
    {
        private readonly InteractiveFloorManagementContext _context;

        public GameCategoryRepository(InteractiveFloorManagementContext context)
        {
            _context = context;
        }

        public async Task<List<GameCategory>> GetAllAsync()
        {
            return await _context.GameCategories.ToListAsync();
        }

        public async Task<GameCategory?> GetByIdAsync(string id)
        {
            return await _context.GameCategories.FindAsync(id.Trim());
        }

        public async Task<GameCategory> CreateAsync(GameCategory gameCategory)
        {
            gameCategory.Id = Guid.NewGuid().ToString("N"); // Generate a clean GUID without hyphens
            _context.GameCategories.Add(gameCategory);
            await _context.SaveChangesAsync();
            return gameCategory;
        }

        public async Task<GameCategory> UpdateAsync(GameCategory gameCategory)
        {
            gameCategory.Id = gameCategory.Id.Trim(); // Ensure ID is trimmed
            _context.Entry(gameCategory).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return gameCategory;
        }

        public async Task DeleteAsync(string id)
        {
            var gameCategory = await _context.GameCategories.FindAsync(id.Trim());
            if (gameCategory != null)
            {
                _context.GameCategories.Remove(gameCategory);
                await _context.SaveChangesAsync();
            }
        }
    }
} 