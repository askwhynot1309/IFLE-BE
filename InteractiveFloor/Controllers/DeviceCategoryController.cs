using BusinessObjects.DTOs.DeviceCategory.Request;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Service.Services.DeviceCategoryServices;

namespace InteractiveFloor.Controllers
{
    [Route("api/device-categories")]
    [ApiController]
    public class DeviceCategoryController : ControllerBase
    {
        private readonly IDeviceCategoryService _deviceCategoryService;

        public DeviceCategoryController(IDeviceCategoryService deviceCategoryService)
        {
            _deviceCategoryService = deviceCategoryService;
        }

        [HttpPost]
        [Authorize(Roles = "Staff")]
        public async Task<IActionResult> CreateDeviceCategory(DeviceCategoryCreateUpdateRequestModel model)
        {
            await _deviceCategoryService.CreateDeviceCategory(model);
            return Ok("Tạo loại thiết bị thành công.");
        }

        [HttpGet]
        public async Task<IActionResult> GetAllDeviceCategory()
        {
            var response = await _deviceCategoryService.GetAllDeviceCategory();
            return Ok(response);
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> GetDeviceCategoryInfo(string id)
        {
            var response = await _deviceCategoryService.GetDeviceCategoryInfo(id);
            return Ok(response);
        }

        [HttpDelete]
        [Route("{id}")]
        [Authorize(Roles = "Staff")]
        public async Task<IActionResult> DeleteDeviceCategory(string id)
        {
            await _deviceCategoryService.DeleteDeviceCategory(id);
            return Ok("Xóa loại thiết bị thành công.");
        }

        [HttpPut]
        [Route("{id}")]
        [Authorize(Roles = "Staff")]
        public async Task<IActionResult> UpdateDeviceCategory(string id, DeviceCategoryCreateUpdateRequestModel model)
        {
            await _deviceCategoryService.UpdateDeviceCategory(id, model);
            return Ok("Cập nhật thông tin loại thiết bị thành công.");
        }
    }
}
