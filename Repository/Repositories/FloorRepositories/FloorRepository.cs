using BusinessObjects.Models;
using DAO;
using Repository.Enums;
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

        public async Task<List<InteractiveFloor>> GetAllPublicFloorsOfOrganization(string organizationId)
        {
            var list = await Get(f => f.IsPublic == true && f.OrganizationId.Equals(organizationId), includeProperties: "Device,Device.DeviceCategory");
            return list.ToList();
        }

        public async Task<List<InteractiveFloor>> GetAllPrivateFloorsOfOrganization(string organizationId)
        {
            var list = await Get(f => f.IsPublic == false && f.OrganizationId.Equals(organizationId), includeProperties: "Device,Device.DeviceCategory");
            return list.ToList();
        }

        public async Task<bool> IsFloorNameExistInOrganization(string organizationId, string name)
        {
            var list = await Get(f => f.OrganizationId.Equals(organizationId));
            var names = list.Select(f => f.Name.ToLower());
            return names.Any(n => n.Equals(name.ToLower()));
        }
    }
}
