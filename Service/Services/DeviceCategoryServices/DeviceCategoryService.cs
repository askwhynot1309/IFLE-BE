﻿using AutoMapper;
using BusinessObjects.DTOs.DeviceCategory.Request;
using BusinessObjects.DTOs.DeviceCategory.Response;
using BusinessObjects.Models;
using Repository.Enums;
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
            if (model.MinDetectionRange >= model.MaxDetectionRange)
            {
                throw new CustomException("Không thể nhập giá trị min lớn hơn hoặc bằng max.");
            }

            var check = await _deviceCategoryRepository.IsNameExisted(model.Name);
            if (check)
            {
                throw new CustomException("Tên danh mục thiết bị này đã tồn tại.");
            }

            var newDeviceCategory = _mapper.Map<DeviceCategory>(model);
            newDeviceCategory.Id = Guid.NewGuid().ToString();
            newDeviceCategory.Status = DeviceStatusEnums.Active.ToString();

            await _deviceCategoryRepository.Insert(newDeviceCategory);
        }

        public async Task<List<DeviceCategoryInfoResponseModel>> GetAllDeviceCategory()
        {
            var list = await _deviceCategoryRepository.GetAllDeviceCategory();
            return _mapper.Map<List<DeviceCategoryInfoResponseModel>>(list.OrderBy(l => l.Name));
        }

        public async Task<List<DeviceCategoryInfoResponseModel>> GetActiveDeviceCategory()
        {
            var list = await _deviceCategoryRepository.GetActiveDeviceCategories();
            return _mapper.Map<List<DeviceCategoryInfoResponseModel>>(list.OrderBy(l => l.Name));
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

        public async Task UpdateDeviceCategory(string id, DeviceCategoryCreateUpdateRequestModel model)
        {

            var deviceCategory = await _deviceCategoryRepository.GetDeviceCategoryById(id);
            if (deviceCategory == null)
            {
                throw new CustomException("Không tìm thấy loại thiết bị này.");
            }
            if (!deviceCategory.Name.ToLower().Equals(model.Name.ToLower()))
            {
                var check = await _deviceCategoryRepository.IsNameExisted(model.Name);
                if (check)
                {
                    throw new CustomException("Tên danh mục thiết bị này đã tồn tại.");
                }
            }
            _mapper.Map(model, deviceCategory);
            if (model.UpdateStatus != null)
            {
                deviceCategory.Status = model.UpdateStatus;
            }
            await _deviceCategoryRepository.Update(deviceCategory);
        }
    }
}
