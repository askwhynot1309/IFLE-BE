using AutoMapper;
using BusinessObjects.DTOs.Device.Request;
using BusinessObjects.DTOs.DeviceCategory.Response;
using BusinessObjects.DTOs.InteractiveFloor.Request;
using BusinessObjects.DTOs.InteractiveFloor.Response;
using BusinessObjects.Models;
using Repository.Enums;
using Repository.Repositories.DeviceCategoryRepositories;
using Repository.Repositories.DeviceRepositories;
using Repository.Repositories.FloorRepositories;
using Repository.Repositories.FloorUserRepositories;
using Repository.Repositories.OrganizationRepositories;
using Repository.Repositories.OrganizationUserRepositories;
using Service.Ultis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Services.FloorServices
{
    public class FloorService : IFloorService
    {
        private readonly IFloorRepository _floorRepository;
        private readonly IMapper _mapper;
        private readonly IOrganizationRepository _organizationRepository;
        private readonly IDeviceRepository _deviceRepository;
        private readonly IDeviceCategoryRepository _deviceCategoryRepository;
        private readonly IFloorUserRepository _floorUserRepository;
        private readonly IOrganizationUserRepository _organizationUserRepository;

        public FloorService(IFloorRepository floorRepository, IMapper mapper, IOrganizationRepository organizationRepository, IDeviceRepository deviceRepository, IDeviceCategoryRepository deviceCategoryRepository, IFloorUserRepository floorUserRepository, IOrganizationUserRepository organizationUserRepository)
        {
            _floorRepository = floorRepository;
            _mapper = mapper;
            _organizationRepository = organizationRepository;
            _deviceRepository = deviceRepository;
            _deviceCategoryRepository = deviceCategoryRepository;
            _floorUserRepository = floorUserRepository;
            _organizationUserRepository = organizationUserRepository;
        }

        public async Task CreateFloor(FloorCreateUpdateRequestModel model, string organizationId)
        {
            var newFloor = _mapper.Map<InteractiveFloor>(model);
            newFloor.Id = Guid.NewGuid().ToString();
            newFloor.Status = FloorStatusEnums.Active.ToString();

            var organization = await _organizationRepository.GetOrganizationById(organizationId);
            if (organization == null)
            {
                throw new CustomException("Không tìm thấy tổ chức này.");
            }
            newFloor.OrganizationId = organizationId;

            await _floorRepository.Insert(newFloor);
        }

        public async Task<FloorDetailsInfoResponseModel> ViewFloorDetailInfo(string floorId)
        {
            var floor = await _floorRepository.GetFloorById(floorId);
            if (floor == null)
            {
                throw new CustomException("Không tìm thấy sàn tương tác này");
            }
            var result = new FloorDetailsInfoResponseModel
            {
                Id = floor.Id,
                Name = floor.Name,
                Description = floor.Description,
                Height = floor.Height,
                Length = floor.Length,
                Width = floor.Width,
                IsPublic = floor.IsPublic,
                Status = floor.Status,
                DeviceInfo = _mapper.Map<DeviceInfo>(floor.Device),
            };
            result.DeviceInfo.DeviceCategory = _mapper.Map<DeviceCategoryInfoResponseModel>(floor.Device.DeviceCategory);
            return result;
        }

        public async Task SoftRemoveFloor(string floorId)
        {
            var floor = await _floorRepository.GetFloorById(floorId);
            if (floor == null)
            {
                throw new CustomException("Không tìm thấy sàn tương tác này");
            }
            floor.Status = FloorStatusEnums.Inactive.ToString();
            await _floorRepository.Update(floor);
        }

        public async Task UpdateFloor(FloorCreateUpdateRequestModel model, string floorId)
        {
            var floor = await _floorRepository.GetFloorById(floorId);
            if (floor == null)
            {
                throw new CustomException("Không tìm thấy sàn này.");
            }
            _mapper.Map(model, floor);
            await _floorRepository.Update(floor);
        }

        public async Task<string> AddDeviceToFloor(string floorId, DeviceCreateUpdateRequestModel model)
        {
            var floor = await _floorRepository.GetFloorById(floorId);
            if (floor == null)
            {
                throw new CustomException("Không tìm thấy sàn tương tác này.");
            }

            var newDevice = _mapper.Map<Device>(model);
            newDevice.Id = Guid.NewGuid().ToString();

            bool hasDeviceCategory = !string.IsNullOrEmpty(model.DeviceCategoryId);
            string message;

            if (hasDeviceCategory)
            {
                var deviceCategory = await _deviceCategoryRepository.GetDeviceCategoryById(model.DeviceCategoryId);
                if (deviceCategory == null)
                {
                    throw new CustomException("Không tìm thấy loại thiết bị này");
                }
                message = "Thêm thiết bị thành công.";
            }
            else
            {
                message = "Thêm thiết bị thành công. Lưu ý: Cần chọn loại thiết bị để chúng tôi biết bạn đang xài loại thiết bị nào nhằm đưa ra hướng dẫn phù hợp.";
            }

            await _deviceRepository.Insert(newDevice);
            floor.DeviceId = newDevice.Id;
            await _floorRepository.Update(floor);

            return message;
        }

        public async Task<DeviceInfo> ViewDeviceInfoOfFloor(string floorId)
        {
            var floor = await _floorRepository.GetFloorById(floorId);
            if (floor == null)
            {
                throw new CustomException("Không tìm thấy sàn tương tác này");
            }

            if (floor.DeviceId == null)
            {
                throw new CustomException("Sàn tương tác này chưa được thêm thiết bị");
            }

            var device = await _deviceRepository.GetDeviceById(floor.DeviceId);
            var result = new DeviceInfo
            {
                Id = device.Id,
                Name = device.Name,
                Description = device.Description,
                Uri = device.Uri,
                DeviceCategory = _mapper.Map<DeviceCategoryInfoResponseModel>(device.DeviceCategory)
            };
            return result;
        }

        public async Task UpdateDeviceInFloor(string floorId, DeviceCreateUpdateRequestModel model)
        {
            var floor = await _floorRepository.GetFloorById(floorId);
            if (floor == null)
            {
                throw new CustomException("Không tìm thấy sàn tương tác này");
            }

            if (floor.DeviceId == null)
            {
                throw new CustomException("Sàn tương tác này chưa được thêm thiết bị");
            }

            var device = await _deviceRepository.GetDeviceById(floor.DeviceId);
            _mapper.Map(model, device);
            await _deviceRepository.Update(device);
        }

        public async Task RemoveDeviceFromFloor(string floorId)
        {
            var floor = await _floorRepository.GetFloorById(floorId);
            if (floor == null)
            {
                throw new CustomException("Không tìm thấy sàn tương tác này");
            }

            if (floor.DeviceId == null)
            {
                throw new CustomException("Sàn tương tác này chưa được thêm thiết bị");
            }
            var device = await _deviceRepository.GetDeviceById(floor.DeviceId);
            floor.DeviceId = null;
            floor.Device = null;

            await _floorRepository.Update(floor);
            await _deviceRepository.Delete(device);
        }

        public async Task AddUserToPrivateFloor(List<string> userIdList, string floorId)
        {
            var floor = await _floorRepository.GetFloorById(floorId);
            if (floor.IsPublic == false)
            {
                throw new CustomException("Sàn tương tác này không phải riêng tư.");
            }

            var userInOrganization = await _organizationUserRepository.GetUserOfOrganization(floor.OrganizationId);

            bool check = userIdList.All(u => userInOrganization.Contains(u));
            if (!check)
            {
                throw new CustomException("Người dùng không thuộc tổ chức chứa sàn tương tác này.");
            }

            var existedList = await _floorUserRepository.GetListUserIdByFloorId(floorId);
            var duplicateUser = existedList.Intersect(userIdList);
            if (duplicateUser.Count() > 0)
            {
                throw new CustomException("Người dùng đã được thêm vào sàn tương tác trước đó.");
            }

            var floorUserList = new List<FloorUser>();
            foreach (var userId in userIdList)
            {
                floorUserList.Add(new FloorUser
                {
                    Id = Guid.NewGuid().ToString(),
                    UserId = userId,
                });
            }

            await _floorUserRepository.InsertRange(floorUserList);
        }
    }
}
