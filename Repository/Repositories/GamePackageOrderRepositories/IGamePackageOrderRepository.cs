﻿using BusinessObjects.DTOs.GamePackage.Response;
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
        Task<List<GamePackageOrder>> GetAllGamePackageOrderOfFloor(string floorId);

        Task<List<GamePackageOrder>> GetAvailableOrderListByPackageId(string packageId);

        Task<GamePackageOrder> GetGamePackageOrderByOrderCode(string orderCode);

        Task<List<GamePackageOrder>> GetAvailableGamePackage(string floorId, DateTime date);

        Task<List<GamePackageOrder>> GetUnactivatedGamePackage(string floorId, DateTime date);

        Task<List<GamePackageOrder>> GetPlayableGamePackageOrderOfGamePackage(string floorId, DateTime date, string gamePackageId);

        Task<List<GamePackageOrder>> GetPlayableGamePackage(string floorId, DateTime date);

        Task<bool> CheckIfAnyAvailableGamePackageInFloorList(List<string> floorIdList, DateTime date);

        Task<bool> CheckIfAnyAvailableGamePackageInFloor(string floorId, DateTime date);

        Task<GamePackageOrder> GetGamePackageOrderById(string id);

        Task<List<GamePackageOrder>> GetOwnGamePackageOrder(string userId);

        Task<List<GamePackageOrder>> GetPendingAndProcessingGamePackageOrder(DateTime now);

        Task<List<GamePackageOrder>> GetInactiveGamePackageOrderOver7Days(DateTime now);

        Task<List<GamePackageOrder>> GetAllOrders();

    }
}
