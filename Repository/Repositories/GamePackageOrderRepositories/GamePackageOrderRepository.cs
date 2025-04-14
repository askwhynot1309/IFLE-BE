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

        public async Task<GamePackageOrder> GetGamePackageOrderByOrderCode(string orderCode)
        {
            return await GetSingle(p => p.OrderCode.Equals(orderCode));
        }

        public async Task<List<GamePackageOrder>> GetAvailableGamePackage(string floorId, DateTime date)
        {
            var list = await Get(
                g => g.FloorId == floorId &&
                     g.Status.Equals(PackageOrderStatusEnums.PAID.ToString()) &&
                     g.EndTime.HasValue &&
                     DateOnly.FromDateTime(g.EndTime.Value) >= DateOnly.FromDateTime(date),
                includeProperties: "GamePackage,GamePackage.GamePackageRelations,GamePackage.GamePackageRelations.Game"
            );

            return list.ToList();
        }

    }
}
