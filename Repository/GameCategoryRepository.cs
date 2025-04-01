using BusinessObjects.Models;
using DAO;
using Microsoft.EntityFrameworkCore;

namespace Repository
{
    public interface IGameCategoryRepository
    {
        Task<List<GameCategory>> GetAllAsync();
        Task<GameCategory> GetByIdAsync(string id);
        Task<GameCategory> CreateAsync(GameCategory gameCategory);
        Task<GameCategory> UpdateAsync(GameCategory gameCategory);
        Task DeleteAsync(string id);
    }

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

        public async Task<GameCategory> GetByIdAsync(string id)
        {
            return await _context.GameCategories.FindAsync(id);
        }

        public async Task<GameCategory> CreateAsync(GameCategory gameCategory)
        {
            _context.GameCategories.Add(gameCategory);
            await _context.SaveChangesAsync();
            return gameCategory;
        }

        public async Task<GameCategory> UpdateAsync(GameCategory gameCategory)
        {
            _context.Entry(gameCategory).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return gameCategory;
        }

        public async Task DeleteAsync(string id)
        {
            var gameCategory = await _context.GameCategories.FindAsync(id);
            if (gameCategory != null)
            {
                _context.GameCategories.Remove(gameCategory);
                await _context.SaveChangesAsync();
            }
        }
    }
} 