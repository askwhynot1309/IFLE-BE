using BusinessObjects.DTOs.DeviceCategory.Request;
using BusinessObjects.DTOs.DeviceCategory.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Services.DeviceCategoryServices
{
    public interface IDeviceCategoryService
    {
        Task CreateDeviceCategory(DeviceCategoryCreateUpdateRequestModel model);

        Task<List<DeviceCategoryInfoResponseModel>> GetAllDeviceCategory();

        Task<List<DeviceCategoryInfoResponseModel>> GetActiveDeviceCategory();

        Task<DeviceCategoryInfoResponseModel> GetDeviceCategoryInfo(string id);

        Task DeprecateDeviceCategory(string id);

        Task UpdateDeviceCategory(string id, DeviceCategoryCreateUpdateRequestModel model);

    }
}
