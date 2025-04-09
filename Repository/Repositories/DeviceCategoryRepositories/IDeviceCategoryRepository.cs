using BusinessObjects.Models;
using Repository.Repositories.GenericRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Repositories.DeviceCategoryRepositories
{
    public interface IDeviceCategoryRepository : IGenericRepository<DeviceCategory>
    {
        Task<DeviceCategory> GetDeviceCategoryById(string id);

        Task<List<DeviceCategory>> GetAllDeviceCategory();

    }
}
