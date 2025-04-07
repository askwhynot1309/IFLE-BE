using BusinessObjects.DTOs.Organization.Request;
using BusinessObjects.DTOs.Organization.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Services.OrganizationServices
{
    public interface IOrganizationService
    {
        Task CreateOrganization(OrganizationCreateUpdateRequestModel model, string userId);

        Task<List<OrganizationInfoResponseModel>> ViewAllOrganizations();

        Task<List<OrganizationUserReponseModel>> GetMembersOfOrganization(string organizationId);

        Task<List<OrganizationInfoResponseModel>> GetOwnOrganization(string userId);

        Task UpdateOrganization(string id, OrganizationCreateUpdateRequestModel model);

        Task AddUserToOrganization(List<string> emailList, string organizationId);

        Task RemoveMemberFromOrganization(List<string> userIdList, string organizationId);

        Task GrantPrivilege(List<string> userIdList, string organizationId);

        Task RemovePrivilege(List<string> userIdList, string organizationId);
    }
}
