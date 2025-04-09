using BusinessObjects.Models;
using Repository.Repositories.GenericRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Repositories.GamePackageRepositories
{
    public interface IGamePackageRepository : IGenericRepository<GamePackage>
    {
        Task<List<GamePackage>> GetAllGamePackages();

        Task<List<GamePackage>> GetActiveGamePackages();

        Task<GamePackage> GetGamePackageById(string id);
    }
}
