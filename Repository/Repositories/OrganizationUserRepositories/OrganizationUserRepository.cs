using BusinessObjects.Models;
using DAO;
using Repository.Repositories.GenericRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Repositories.OrganizationUserRepositories
{
    public class OrganizationUserRepository : GenericRepository<OrganizationUser>, IOrganizationUserRepository
    {
        public OrganizationUserRepository(InteractiveFloorManagementContext context) : base(context)
        {
        }

        public async Task<List<string>> GetUserOfOrganization(string organizationId)
        {
            var organizationUsers = await Get(u => u.OrganizationId.Equals(organizationId));
            var userListId = organizationUsers.Select(u => u.UserId).ToList();
            return userListId;
        }

        public async Task<List<Organization>> GetOrganizationOfUser(string userId)
        {
            var organizationUsers = await Get(u => u.UserId.Equals(userId), includeProperties: "Organization");
            var organizationList = organizationUsers.Select(u => u.Organization).ToList();
            return organizationList;
        }
    }
}
