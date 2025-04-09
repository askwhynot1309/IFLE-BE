using BusinessObjects.Models;
using Repository.Repositories.GenericRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Repositories.RoleRepositories
{
    public interface IRoleRepository : IGenericRepository<Role>
    {
        Task<string> GetRoleIdByName(string name);
    }
}
