using BusinessObjects.DTOs.Device.Request;
using BusinessObjects.DTOs.InteractiveFloor.Request;
using BusinessObjects.DTOs.InteractiveFloor.Response;
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

        Task<FloorDetailsInfoResponseModel> ViewFloorDetailInfo(string floorId);

        Task UpdateFloor(FloorCreateUpdateRequestModel model, string floorId);

        Task SoftRemoveFloor(string floorId);

        Task<string> AddDeviceToFloor(string floorId, DeviceCreateUpdateRequestModel model);

        Task<DeviceInfo> ViewDeviceInfoOfFloor(string floorId);

        Task UpdateDeviceInFloor(string floorId, DeviceCreateUpdateRequestModel model);

        Task RemoveDeviceFromFloor(string floorId);

        Task AddUserToPrivateFloor(List<string> userIdList, string floorId, string currentUserId);

        Task<List<UserInfoResponeModel>> GetUserInPrivateFloor(string floorId);

        Task RemoveUserFromPrivateFloor(string floorId, List<string> userIdList);
    }
}
