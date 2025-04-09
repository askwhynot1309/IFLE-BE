using BusinessObjects.Models;
using DAO;
using Repository.Repositories.GenericRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Repositories.RoleRepositories
{
    public class RoleRepository : GenericRepository<Role>, IRoleRepository
    {
        public RoleRepository(InteractiveFloorManagementContext context) : base(context)
        {
        }

        public async Task<string> GetRoleIdByName(string name)
        {
            return (await GetSingle(r => r.Name.ToLower().Equals(name.ToLower()))).Id;
        }
    }

}
