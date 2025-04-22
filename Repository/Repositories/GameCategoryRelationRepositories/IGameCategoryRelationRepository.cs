using BusinessObjects.Models;
using Repository.Repositories.GenericRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Repositories.GameCategoryRelationRepositories
{
    public interface IGameCategoryRelationRepository : IGenericRepository<GameCategoryRelation>
    {
        Task<List<GameCategoryRelation>> GetListByGameCategory(string gameCategoryId);
    }
}
