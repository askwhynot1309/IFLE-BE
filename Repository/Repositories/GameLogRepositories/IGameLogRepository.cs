using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObjects.Models;
using Repository.Repositories.GenericRepositories;

namespace Repository.Repositories.GameLogRepositories
{
    public interface IGameLogRepository : IGenericRepository<GameLog>
    {
        public Task<IEnumerable<GameLog>> GetGameLogsByGameId(string gameId);
    }
}
