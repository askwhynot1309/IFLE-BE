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
    public class FloorUserRepository : GenericRepository<FloorUser>, IFloorUserRepository
    {
        public FloorUserRepository(InteractiveFloorManagementContext context) : base(context)
        {
        }

        public async Task<List<string>> GetListUserIdByFloorId(string floorId)
        {
            return (await Get(u => u.FloorId.Equals(floorId))).Select(u => u.UserId).ToList();
        }
    }
}
