using BusinessObjects.Models;
using Repository.Repositories.GenericRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Repositories.UserPackageRepositories
{
    public interface IUserPackageRepository : IGenericRepository<UserPackage>
    {
        Task<List<UserPackage>> GetAllUserPackages();

        Task<List<UserPackage>> GetActiveUserPackages();

        Task<UserPackage> GetUserPackageById(string id);

        Task<bool> IsNameExisted(string name);
    }
}
