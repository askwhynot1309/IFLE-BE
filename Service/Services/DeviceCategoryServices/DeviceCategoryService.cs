using AutoMapper;
using BusinessObjects.DTOs.DeviceCategory.Request;
using BusinessObjects.DTOs.DeviceCategory.Response;
using BusinessObjects.Models;
using Repository.Repositories.DeviceCategoryRepositories;
using Service.Ultis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Services.DeviceCategoryServices
{
    public class DeviceCategoryService : IDeviceCategoryService
    {
        private readonly IDeviceCategoryRepository _deviceCategoryRepository;
        private readonly IMapper _mapper;

        public DeviceCategoryService(IDeviceCategoryRepository deviceCategoryRepository, IMapper mapper)
        {
            _deviceCategoryRepository = deviceCategoryRepository;
            _mapper = mapper;
        }

        public async Task CreateDeviceCategory(DeviceCategoryCreateUpdateRequestModel model)
        {
            var newDeviceCategory = _mapper.Map<DeviceCategory>(model);
            newDeviceCategory.Id = Guid.NewGuid().ToString();

            await _deviceCategoryRepository.Insert(newDeviceCategory);
        }

        public async Task<List<DeviceCategoryInfoResponseModel>> GetAllDeviceCategory()
        {
            var list = await _deviceCategoryRepository.GetAllDeviceCategory();
            return _mapper.Map<List<DeviceCategoryInfoResponseModel>>(list);
        }

        public async Task<DeviceCategoryInfoResponseModel> GetDeviceCategoryInfo(string id)
        {
            var deviceCategory = await _deviceCategoryRepository.GetDeviceCategoryById(id);
            if (deviceCategory == null)
            {
                throw new CustomException("Không tìm thấy loại thiết bị này.");
            }
            return _mapper.Map<DeviceCategoryInfoResponseModel>(deviceCategory);
        }

        public async Task DeleteDeviceCategory(string id)
        {
            var deviceCategory = await _deviceCategoryRepository.GetDeviceCategoryById(id);
            if (deviceCategory == null)
            {
                throw new CustomException("Không tìm thấy loại thiết bị này.");
            }

            foreach (var device in deviceCategory.Devices)
            {
                device.DeviceCategoryId = null;
                device.DeviceCategory = null;
            }

            deviceCategory.Devices.Clear();
            await _deviceCategoryRepository.Delete(deviceCategory);
        }

        public async Task UpdateDeviceCategory(string id, DeviceCategoryCreateUpdateRequestModel model)
        {
            var deviceCategory = await _deviceCategoryRepository.GetDeviceCategoryById(id);
            if (deviceCategory == null)
            {
                throw new CustomException("Không tìm thấy loại thiết bị này.");
            }
            _mapper.Map(model, deviceCategory);
            await _deviceCategoryRepository.Update(deviceCategory);
        }
    }
}
