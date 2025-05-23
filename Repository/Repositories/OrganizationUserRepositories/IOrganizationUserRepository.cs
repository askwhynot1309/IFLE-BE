﻿using BusinessObjects.Models;
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

        Task<List<OrganizationUser>> GetOrganizationUsersByUserIdList(List<string> userIdList, string organizationId);

        Task<List<OrganizationUser>> GetOrganizationUserByOrganizationId(string organizationId);

        Task<OrganizationUser> GetOrganizationUserByUserIdAndOrganizationId(string userId, string organizationId);

        Task<string> GetOwnerIdOfOrganization(string organizationId);

        Task<List<OrganizationUser>> GetOrganizationUserListByUserId(string userId);

        Task<List<string>> GetOwnerAndCoownerIdListOfOrganization(string organizationId);

        Task<bool> IsCreatedOrganizationNameExist(string userId, string name);

    }
}
