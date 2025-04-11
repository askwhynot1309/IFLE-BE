using BusinessObjects.Models;
using DAO;
using Repository.Enums;
using Repository.Repositories.GenericRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Repositories.GamePackageOrderRepositories
{
    public class GamePackageOrderRepository : GenericRepository<GamePackageOrder>, IGamePackageOrderRepository
    {
        public GamePackageOrderRepository(InteractiveFloorManagementContext context) : base(context)
        {
        }

        public async Task<List<GamePackageOrder>> GetAvailableOrderListByPackageId(string packageId)
        {
            var statusList = new List<string> { PackageOrderStatusEnums.PAID.ToString(), PackageOrderStatusEnums.PENDING.ToString(), PackageOrderStatusEnums.PROCESSING.ToString() };

            var list = await Get(g => g.GamePackageId.Equals(packageId) && statusList.Contains(g.Status));
            return list.ToList();
        }
    }
}
