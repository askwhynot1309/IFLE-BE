using BusinessObjects.Models;
using DAO;
using Repository.Enums;
using Repository.Repositories.GenericRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Repositories.GamePackageRepositories
{
    public class GamePackageRepository : GenericRepository<GamePackage>, IGamePackageRepository
    {
        public GamePackageRepository(InteractiveFloorManagementContext context) : base(context)
        {
        }

        public async Task<List<GamePackage>> GetAllGamePackages()
        {
            return (await Get(includeProperties: "GamePackageRelations,GamePackageRelations.Game")).ToList();
        }

        public async Task<List<GamePackage>> GetActiveGamePackages()
        {
            return (await Get(g => g.Status.Equals(GamePackageEnums.Active.ToString()), includeProperties: "GamePackageRelations,GamePackageRelations.Game")).ToList();
        }

        public async Task<GamePackage> GetGamePackageById(string id)
        {
            return await GetSingle(g => g.Id.Equals(id), includeProperties: "GamePackageRelations");
        }
    }
}
