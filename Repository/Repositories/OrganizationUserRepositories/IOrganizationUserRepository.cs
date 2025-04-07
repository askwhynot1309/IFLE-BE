using BusinessObjects.Models;
using Repository.Repositories.GenericRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Repositories.OrganizationUserRepositories
{
    public interface IOrganizationUserRepository : IGenericRepository<OrganizationUser>
    {
        Task<List<string>> GetUserOfOrganization(string organizationId);

        Task<List<Organization>> GetOrganizationOfUser(string userId);
    }
}
