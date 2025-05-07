using BusinessObjects.DTOs.UserPackage.Response;
using BusinessObjects.Models;
using DAO;
using Repository.Enums;
using Repository.Repositories.GenericRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Repositories.UserPackageRepositories
{
    public class UserPackageRepository : GenericRepository<UserPackage>, IUserPackageRepository
    {
        public UserPackageRepository(InteractiveFloorManagementContext context) : base(context)
        {
        }

        public async Task<List<UserPackage>> GetAllUserPackages()
        {
            return (await Get()).ToList();
        }

        public async Task<List<UserPackage>> GetActiveUserPackages()
        {
            return (await Get(u => u.Status.Equals(UserPackageEnums.Active.ToString()))).ToList();
        }

        public async Task<UserPackage> GetUserPackageById(string id)
        {
            return await GetSingle(u => u.Id.Equals(id));
        }

        public async Task<bool> IsNameExisted(string name)
        {
            return (await GetSingle(g => g.Name.ToLower().Equals(name.ToLower()))) != null;
        }
    }
}
