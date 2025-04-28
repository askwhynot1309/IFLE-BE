using BusinessObjects.DTOs.Device.Request;
using BusinessObjects.DTOs.GamePackage.Response;
using BusinessObjects.DTOs.GamePackageOrder.Request;
using BusinessObjects.DTOs.GamePackageOrder.Response;
using BusinessObjects.DTOs.InteractiveFloor.Request;
using BusinessObjects.DTOs.InteractiveFloor.Response;
using BusinessObjects.DTOs.SetUpGuide.Request;
using BusinessObjects.DTOs.SetUpGuide.Response;
using BusinessObjects.DTOs.User.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Services.FloorServices
{
    public interface IFloorService
    {
        Task CreateFloor(FloorCreateUpdateRequestModel model, string organizationId, string userId);

        Task<FloorDetailsInfoResponseModel> ViewFloorDetailInfo(string floorId, string currentUserId);

        Task UpdateFloor(FloorCreateUpdateRequestModel model, string floorId, string currentUserId);

        Task SoftRemoveFloor(string floorId);

        Task<string> AddDeviceToFloor(string floorId, DeviceCreateUpdateRequestModel model);

        Task<DeviceInfo> ViewDeviceInfoOfFloor(string floorId);

        Task UpdateDeviceInFloor(string floorId, DeviceCreateUpdateRequestModel model);

        Task RemoveDeviceFromFloor(string floorId);

        Task AddUserToPrivateFloor(List<string> userIdList, string floorId, string currentUserId);

        Task<List<UserInfoResponeModel>> GetUserInPrivateFloor(string floorId);

        Task RemoveUserFromPrivateFloor(string floorId, List<string> userIdList);

        Task<string> BuyGamePackageForFloor(string floorId, GamePackageOrderCreateRequestModel model, string currentUserId);

        Task UpdateGamePackageOrderStatus(string orderCode, string currentUserId);

        Task<List<GamePackageOrderDetailsResponseModel>> GetAllAvailableGamePackageOfFloor(string floorId);

        Task<List<GamePackageOrderDetailsResponseModel>> GetPlayableGamePackageOfFloor(string floorId);

        Task ActivateGamePackageOrder(string gamePackageOrderId);

        Task<List<GamePackageOrderListResponseModel>> GetGamePackageOrderOfFloor(string id);

        Task<string> CreateAgainPaymentUrlForPendingGamePackageOrder(string orderId);

        Task<GamePackageOrderDetailsResponseModel> GetGamePackageOrderDetails(string orderId);

        Task AutoUpdateGamePackageOrderStatus();

        Task AutoActivateGamePackageOrderOver7Days();

        Task<SetUpGuideResponseModel> GetSetUpGuideForCustomer(SetUpGuideRequestModel model, string floorId);
    }
}
