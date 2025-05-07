using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObjects.Models;
using DAO;
using Microsoft.EntityFrameworkCore;
using Repository.Repositories.GenericRepositories;

namespace Repository.Repositories.GameLogRepositories
{
    public class GameLogRepository : GenericRepository<GameLog>, IGameLogRepository
    {
        private readonly InteractiveFloorManagementContext _context;

        public GameLogRepository(InteractiveFloorManagementContext context) : base(context)
        {
            _context = context;
        }

        public async Task<IEnumerable<GameLog>> GetGameLogsByGameId(string gameId)
        {
            return await _context.GameLogs
                .Include(g => g.Staff)
                .Include(g => g.Game)
                .Where(g => g.GameId == gameId)
                .ToListAsync();
        }
    }
}
