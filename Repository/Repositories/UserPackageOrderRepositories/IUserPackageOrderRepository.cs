using BusinessObjects.Models;
using Repository.Repositories.GenericRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Repositories.UserPackageOrderRepositories
{
    public interface IUserPackageOrderRepository : IGenericRepository<UserPackageOrder>
    {
        Task<List<UserPackageOrder>> GetAllUserPackageOrderOfOrganization(string organizationId);

        Task<List<UserPackageOrder>> GetAvailableOrderListByPackageId(string packageId);

        Task<UserPackageOrder> GetUserPackageOrderByOrderCode(string orderCode);

        Task<List<UserPackageOrder>> GetOwnUserPackageOrder(string userId);

        Task<UserPackageOrder> GetUserPackageOrderById(string orderId);

    }
}
