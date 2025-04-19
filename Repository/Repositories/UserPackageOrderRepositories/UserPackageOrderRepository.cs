using BusinessObjects.Models;
using DAO;
using Repository.Enums;
using Repository.Repositories.GenericRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Repositories.UserPackageOrderRepositories
{
    public class UserPackageOrderRepository : GenericRepository<UserPackageOrder>, IUserPackageOrderRepository
    {
        public UserPackageOrderRepository(InteractiveFloorManagementContext context) : base(context)
        {
        }

        public async Task<List<UserPackageOrder>> GetAllUserPackageOrderOfOrganization(string organizationId)
        {
            return (await Get(o => o.OrganizationId.Equals(organizationId))).ToList();
        }

        public async Task<List<UserPackageOrder>> GetAvailableOrderListByPackageId(string packageId)
        {
            var statusList = new List<string> { PackageOrderStatusEnums.PAID.ToString(), PackageOrderStatusEnums.PENDING.ToString(), PackageOrderStatusEnums.PROCESSING.ToString() };

            var list = await Get(g => g.UserPackageId.Equals(packageId) && statusList.Contains(g.Status));
            return list.ToList();
        }

        public async Task<UserPackageOrder> GetUserPackageOrderByOrderCode(string orderCode)
        {
            return await GetSingle(p => p.OrderCode.Equals(orderCode));
        }

        public async Task<List<UserPackageOrder>> GetOwnUserPackageOrder(string userId)
        {
            var ownUserPackageOrders = await Get(o => o.UserId.Equals(userId));
            return ownUserPackageOrders.ToList();
        }

        public async Task<UserPackageOrder> GetUserPackageOrderById(string orderId)
        {
            return await GetSingle(p => p.Id.Equals(orderId), includeProperties: "UserPackage");
        }
    }
}
