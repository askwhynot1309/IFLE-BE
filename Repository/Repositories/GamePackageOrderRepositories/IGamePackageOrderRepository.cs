using BusinessObjects.DTOs.GamePackage.Response;
using BusinessObjects.Models;
using Repository.Repositories.GenericRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Repositories.GamePackageOrderRepositories
{
    public interface IGamePackageOrderRepository : IGenericRepository<GamePackageOrder>
    {
        Task<List<GamePackageOrder>> GetAvailableOrderListByPackageId(string packageId);

        Task<GamePackageOrder> GetGamePackageOrderByOrderCode(string orderCode);

        Task<List<GamePackageOrder>> GetAvailableGamePackage(string floorId, DateTime date);

    }
}
