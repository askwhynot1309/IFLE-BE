using BusinessObjects.Models;
using DAO;
using Repository.Repositories.GenericRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Repositories.FloorRepositories
{
    public class FloorRepository : GenericRepository<InteractiveFloor>, IFloorRepository
    {
        public FloorRepository(InteractiveFloorManagementContext context) : base(context)
        {
        }

        public async Task<InteractiveFloor> GetFloorById(string floorId)
        {
            return await GetSingle(f => f.Id.Equals(floorId), includeProperties: "Device,Device.DeviceCategory");
        }
    }
}
