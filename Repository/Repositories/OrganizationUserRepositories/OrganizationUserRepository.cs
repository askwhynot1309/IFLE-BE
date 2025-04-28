using BusinessObjects.Models;
using DAO;
using Repository.Enums;
using Repository.Repositories.GenericRepositories;
using System;
using System.Collections.Generic;
using System.Globalization;
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

        public async Task<List<OrganizationUser>> GetOrganizationUsersByUserIdList(List<string> userIdList, string organizationId)
        {
            var list = await Get(o => userIdList.Contains(o.UserId) && o.OrganizationId.Equals(organizationId));
            return list.ToList();
        }

        public async Task<List<OrganizationUser>> GetOrganizationUserByOrganizationId(string organizationId)
        {
            var organizationUsers = await Get(o => o.OrganizationId.Equals(organizationId));
            return organizationUsers.ToList();
        }

        public async Task<OrganizationUser> GetOrganizationUserByUserIdAndOrganizationId(string userId, string organizationId)
        {
            return await GetSingle(o => o.UserId.Equals(userId) && o.OrganizationId.Equals(organizationId));
        }

        public async Task<string> GetOwnerIdOfOrganization(string organizationId)
        {
            return (await GetSingle(o => o.OrganizationId.Equals(organizationId) && o.Privilege.Equals(PrivilegeEnums.Owner.ToString()))).UserId;
        }

        public async Task<List<OrganizationUser>> GetOrganizationUserListByUserId(string userId)
        {
            return (await Get(o => o.UserId.Equals(userId))).ToList();
        }

        public async Task<List<string>> GetOwnerAndCoownerIdListOfOrganization(string organizationId)
        {
            var privilegeList = new List<string> { PrivilegeEnums.Owner.ToString(), PrivilegeEnums.CoOwner.ToString() };
            var list = await Get(o => o.OrganizationId.Equals(organizationId) && privilegeList.Contains(o.Privilege));
            return list.Select(o => o.UserId).ToList();
        }

        public async Task<bool> IsCreatedOrganizationNameExist(string userId, string name)
        {
            var list = await Get(o => o.UserId.Equals(userId) && o.Privilege.Equals(PrivilegeEnums.Owner.ToString()), includeProperties: "Organization");
            var names = list.Select(o => o.Organization.Name.ToLower()).ToList();
            return names.Any(n => n.Equals(name.ToLower()));
        }
    }
}
