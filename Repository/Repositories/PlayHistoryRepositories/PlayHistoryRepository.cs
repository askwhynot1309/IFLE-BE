using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObjects.Models;
using DAO;
using Microsoft.EntityFrameworkCore;
using Repository.Repositories.GenericRepositories;

namespace Repository.Repositories.PlayHistoryRepositories
{
    public class PlayHistoryRepository : GenericRepository<PlayHistory>, IPlayHistoryRepository
    {
        private readonly InteractiveFloorManagementContext _context;

        public PlayHistoryRepository(InteractiveFloorManagementContext context) : base(context)
        {
            _context = context;
        }

        public async Task<PlayHistory> CreateAsync(PlayHistory playHistory)
        {
            _context.PlayHistories.Add(playHistory);    
            await _context.SaveChangesAsync();
            return playHistory;
        }

        public async Task<List<PlayHistory>> GetFloorPlayHistory(string floorId)
        {
            var floorPlayHistory = await _context.PlayHistories
                .Include(ph => ph.Game)
                .Include(ph => ph.User)
                .Where(ph => ph.FloorId == floorId)
                .ToListAsync();
            return floorPlayHistory;
        }

        public async Task<PlayHistory> GetHighScore(string userId, string gameId)
        {
           var history = await _context.PlayHistories
                .Where(ph => ph.UserId == userId && ph.GameId == gameId)
                .OrderByDescending(ph => ph.Score)
                .FirstOrDefaultAsync();
            return history;
        }
    }
}
