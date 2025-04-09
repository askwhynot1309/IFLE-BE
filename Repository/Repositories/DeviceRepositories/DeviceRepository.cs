using BusinessObjects.Models;
using DAO;
using Repository.Repositories.GenericRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Repositories.DeviceRepositories
{
    public class DeviceRepository : GenericRepository<Device>, IDeviceRepository
    {
        public DeviceRepository(InteractiveFloorManagementContext context) : base(context)
        {
        }

        public async Task<Device> GetDeviceById(string id)
        {
            return await GetSingle(d => d.Id.Equals(id), includeProperties: "DeviceCategory");
        }
    }
}
