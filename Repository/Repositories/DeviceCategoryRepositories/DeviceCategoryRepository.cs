using BusinessObjects.Models;
using DAO;
using Repository.Enums;
using Repository.Repositories.GenericRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Repositories.DeviceCategoryRepositories
{
    public class DeviceCategoryRepository : GenericRepository<DeviceCategory>, IDeviceCategoryRepository
    {
        public DeviceCategoryRepository(InteractiveFloorManagementContext context) : base(context)
        {
        }

        public async Task<DeviceCategory> GetDeviceCategoryById(string id)
        {
            return await GetSingle(d => d.Id.Equals(id), includeProperties: "Devices");
        }

        public async Task<List<DeviceCategory>> GetAllDeviceCategory()
        {
            return (await Get()).ToList();
        }

        public async Task<List<DeviceCategory>> GetActiveDeviceCategories()
        {
            return (await Get(d => d.Status.Equals(DeviceStatusEnums.Active.ToString()))).ToList();
        }

        public async Task<bool> IsNameExisted(string name)
        {
            return (await GetSingle(d => d.Name.ToLower().Equals(name.ToLower()))) != null;
        }
    }
}
