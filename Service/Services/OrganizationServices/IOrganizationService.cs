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
        Task CreateOrganization(OrganizationCreateRequestModel model, string userId);

        Task<List<OrganizationInfoResponseModel>> ViewAllOrganizations();

        Task<List<OrganizationUserReponseModel>> GetMembersOfOrganization(string organizationId);

        Task<List<OrganizationInfoResponseModel>> GetOwnOrganization(string userId);
    }
}
