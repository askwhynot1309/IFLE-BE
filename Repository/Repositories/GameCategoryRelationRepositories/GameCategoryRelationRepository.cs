using BusinessObjects.Models;
using DAO;
using Repository.Repositories.GenericRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Repositories.GameCategoryRelationRepositories
{
    public class GameCategoryRelationRepository : GenericRepository<GameCategoryRelation>, IGameCategoryRelationRepository
    {
        public GameCategoryRelationRepository(InteractiveFloorManagementContext context) : base(context)
        {
        }

        public async Task<List<GameCategoryRelation>> GetListByGameCategory(string gameCategoryId)
        {
            return (await Get(g => g.GameCategoryId.Equals(gameCategoryId))).ToList();
        }

    }
}
