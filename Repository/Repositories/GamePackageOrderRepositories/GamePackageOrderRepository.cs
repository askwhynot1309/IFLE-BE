using BusinessObjects.Models;
using DAO;
using Repository.Enums;
using Repository.Repositories.GenericRepositories;
using System;
using System.Collections.Generic;
using System.Formats.Asn1;
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

        public async Task<List<GamePackageOrder>> GetAllGamePackageOrderOfFloor(string floorId)
        {
            return (await Get(g => g.FloorId.Equals(floorId), includeProperties: "GamePackage," +
                "GamePackage.GamePackageRelations,GamePackage.GamePackageRelations.Game")).ToList();
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
                     ((g.EndTime.HasValue && DateOnly.FromDateTime(g.EndTime.Value) >= DateOnly.FromDateTime(date)) ||
                     g.IsActivated == false),
                includeProperties: "GamePackage,GamePackage.GamePackageRelations,GamePackage.GamePackageRelations.Game"
            );
            return list.ToList();
        }

        public async Task<List<GamePackageOrder>> GetPlayableGamePackageOrderOfGamePackage(string floorId, DateTime date, string gamePackageId)
        {
            var list = await Get(
                g => g.FloorId == floorId &&
                     g.GamePackageId.Equals(gamePackageId) &&
                     g.IsActivated == true &&
                     g.Status.Equals(PackageOrderStatusEnums.PAID.ToString()) &&
                     g.EndTime.HasValue && DateOnly.FromDateTime(g.EndTime.Value) >= DateOnly.FromDateTime(date),
                includeProperties: "GamePackage,GamePackage.GamePackageRelations,GamePackage.GamePackageRelations.Game"
            );
            return list.OrderByDescending(o => o.EndTime).ToList();
        }

        public async Task<List<GamePackageOrder>> GetPlayableGamePackage(string floorId, DateTime date)
        {
            var list = await Get(
                g => g.FloorId == floorId &&
                     g.IsActivated == true &&
                     g.Status.Equals(PackageOrderStatusEnums.PAID.ToString()) &&
                     g.EndTime.HasValue && DateOnly.FromDateTime(g.EndTime.Value) >= DateOnly.FromDateTime(date),
                includeProperties: "GamePackage,GamePackage.GamePackageRelations,GamePackage.GamePackageRelations.Game," +
                "GamePackage.GamePackageRelations.Game.GameCategoryRelations,GamePackage.GamePackageRelations.Game.GameCategoryRelations.GameCategory," +
                "GamePackage.GamePackageRelations.Game.GameVersions"
            );

            return list.ToList();
        }

        public async Task<bool> CheckIfAnyAvailableGamePackageInFloorList(List<string> floorIdList, DateTime date)
        {
            var list = await Get(
                g => floorIdList.Contains(g.FloorId) &&
                     g.Status.Equals(PackageOrderStatusEnums.PAID.ToString()) &&
                     ((g.EndTime.HasValue && DateOnly.FromDateTime(g.EndTime.Value) >= DateOnly.FromDateTime(date)) ||
                     g.IsActivated == false));
            return list.Count() > 0;
        }

        public async Task<bool> CheckIfAnyAvailableGamePackageInFloor(string floorId, DateTime date)
        {
            var list = await Get(
                g => g.FloorId.Equals(floorId) &&
                     g.Status.Equals(PackageOrderStatusEnums.PAID.ToString()) &&
                     ((g.EndTime.HasValue && DateOnly.FromDateTime(g.EndTime.Value) >= DateOnly.FromDateTime(date)) ||
                     g.IsActivated == false));
            return list.Count() > 0;
        }

        public async Task<GamePackageOrder> GetGamePackageOrderById(string id)
        {
            return await GetSingle(g => g.Id.Equals(id),
                includeProperties: "GamePackage,GamePackage.GamePackageRelations,GamePackage.GamePackageRelations.Game");
        }

        public async Task<List<GamePackageOrder>> GetOwnGamePackageOrder(string userId)
        {
            var ownGamePackageOrders = await Get(o => o.UserId.Equals(userId));
            return ownGamePackageOrders.ToList();
        }

        public async Task<List<GamePackageOrder>> GetPendingAndProcessingGamePackageOrder(DateTime now)
        {
            var statusList = new List<string>
            {
                PackageOrderStatusEnums.PENDING.ToString(),
                PackageOrderStatusEnums.PROCESSING.ToString(),
            };

            var thresholdTime = now.AddMinutes(-5);

            var list = await Get(l =>
                statusList.Contains(l.Status) &&
                l.OrderDate < thresholdTime
            );
            return list.ToList();
        }

        public async Task<List<GamePackageOrder>> GetInactiveGamePackageOrderOver7Days(DateTime now)
        {
            var list = await Get(l => l.IsActivated == false && DateOnly.FromDateTime(now) > DateOnly.FromDateTime(l.OrderDate).AddDays(7));
            return list.ToList();
        }

        public async Task<List<GamePackageOrder>> GetAllOrders()
        {
            return (await Get(includeProperties: "User,GamePackage")).ToList();
        }

    }
}
