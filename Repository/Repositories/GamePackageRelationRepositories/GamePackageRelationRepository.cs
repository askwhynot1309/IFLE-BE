using BusinessObjects.Models;
using DAO;
using Repository.Repositories.GenericRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Repositories.GamePackageRelationRepositories
{
    public class GamePackageRelationRepository : GenericRepository<GamePackageRelation>, IGamePackageRelationRepository
    {
        public GamePackageRelationRepository(InteractiveFloorManagementContext context) : base(context)
        {
        }

        public async Task<List<string>> GetListGameIdByGamePackageId(string gamePackageId)
        {
            var list = await Get(g => g.GamePackageId.Equals(gamePackageId));
            return list.Select(g => g.GameId).ToList();
        }

        public async Task<List<GamePackageRelation>> GetListByGamePackageId(string gamePackageId)
        {
            var list = await Get(g => g.GamePackageId.Equals(gamePackageId), includeProperties: "Game");
            return list.ToList();
        }

        public async Task<List<GamePackage>> GetListGamePackageByGameId(string gameId)
        {
            var list = (await Get(g => g.GameId.Equals(gameId), includeProperties: "GamePackage")).Select(g => g.GamePackage);
            return list.ToList();
        }
    }
}
