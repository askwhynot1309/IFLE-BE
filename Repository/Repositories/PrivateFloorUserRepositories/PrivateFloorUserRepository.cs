using BusinessObjects.Models;
using DAO;
using Repository.Repositories.GenericRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Repositories.FloorUserRepositories
{
    public class PrivateFloorUserRepository : GenericRepository<PrivateFloorUser>, IPrivateFloorUserRepository
    {
        public PrivateFloorUserRepository(InteractiveFloorManagementContext context) : base(context)
        {
        }

        public async Task<List<string>> GetListUserIdByFloorId(string floorId)
        {
            return (await Get(u => u.FloorId.Equals(floorId))).Select(u => u.UserId).ToList();
        }

        public async Task<List<PrivateFloorUser>> GetListByUserIdAndPrivateFloorIdList(string userId, List<string> privateFloorIdList)
        {
            var list = await Get(p => p.UserId.Equals(userId) && privateFloorIdList.Contains(p.FloorId), includeProperties: "InteractiveFloor,InteractiveFloor.Device,InteractiveFloor.Device.DeviceCategory");
            return list.ToList();
        }

        public async Task<List<PrivateFloorUser>> GetListByUserIdList(List<string> userIdList, string floorId)
        {
            var list = await Get(l => l.FloorId.Equals(floorId) && userIdList.Contains(l.UserId));
            return list.ToList();
        }
    }
}
