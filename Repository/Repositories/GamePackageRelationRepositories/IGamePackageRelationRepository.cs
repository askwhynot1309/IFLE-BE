using BusinessObjects.Models;
using Repository.Repositories.GenericRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Repositories.GamePackageRelationRepositories
{
    public interface IGamePackageRelationRepository : IGenericRepository<GamePackageRelation>
    {
        Task<List<string>> GetListGameIdByGamePackageId(string gamePackageId);

        Task<List<GamePackageRelation>> GetListByGamePackageId(string gamePackageId);
    }
}
