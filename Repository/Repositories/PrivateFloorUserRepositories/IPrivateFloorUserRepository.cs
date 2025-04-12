using BusinessObjects.Models;
using Repository.Repositories.GenericRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Repositories.FloorUserRepositories
{
    public interface IPrivateFloorUserRepository : IGenericRepository<PrivateFloorUser>
    {
        Task<List<string>> GetListUserIdByFloorId(string floorId);

        Task<List<PrivateFloorUser>> GetListByUserIdAndPrivateFloorIdList(string userId, List<string> privateFloorIdList);

        Task<List<PrivateFloorUser>> GetListByUserIdList(List<string> userIdList, string floorId);
    }
}
