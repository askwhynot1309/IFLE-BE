using BusinessObjects.DTOs.InteractiveFloor.Response;
using BusinessObjects.DTOs.Organization.Request;
using BusinessObjects.DTOs.Organization.Response;
using BusinessObjects.DTOs.UserPackage.Response;
using BusinessObjects.DTOs.UserPackageOrder.Request;
using BusinessObjects.DTOs.UserPackageOrder.Response;
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

        Task<List<OrganizationUserReponseModel>> GetMembersOfOrganization(string organizationId, string currentId);

        Task<List<OrganizationInfoResponseModel>> GetOwnOrganization(string userId);

        Task UpdateOrganization(string id, OrganizationCreateUpdateRequestModel model, string currentUserId);

        Task AddUserToOrganization(List<string> emailList, string organizationId, string currentId);

        Task RemoveMemberFromOrganization(List<string> userIdList, string organizationId, string currentId);

        Task GrantPrivilege(List<string> userIdList, string organizationId, string currentUserId);

        Task RemovePrivilege(List<string> userIdList, string organizationId, string currentUserId);

        Task<OrganizationInfoResponseModel> GetDetailsInfoOfOrganization(string organizationId, string currentUserId);

        Task<List<FloorDetailsInfoResponseModel>> GetFloorListOfOrganization(string organizationId, string currentUserId);

        Task<string> BuyUserPackageForOrganization(string organizationId, UserPackageOrderCreateRequestModel model, string currentUserId);

        Task UpdateUserPackageOrderStatus(string orderCode, string currentUserId);

        Task<List<UserPackageOrderListResponseModel>> GetAllUserPackageOrders(string id);

        Task<string> CreateAgainPaymentUrlForPendingUserPackageOrder(string orderId);

        Task<UserPackageOrderDetailsResponseModel> GetUserPackageOrderDetails(string orderId);

        Task AutoUpdateUserPackageOrderStatus();

    }
}
