using BusinessObjects.DTOs.GamePackage.Request;
using BusinessObjects.DTOs.GamePackage.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Services.GamePackageServices
{
    public interface IGamePackageService
    {
        Task CreateGamePackage(GamePackageCreateRequestModel model);

        Task<List<GamePackageListResponseModel>> GetAllGamePackages();

        Task<List<GamePackageListResponseModel>> GetActiveGamePackages();

        Task AddGameToPackage(string packageId, List<string> gameIdList);

        Task<GamePackageDetailsResponseModel> GetGamePackageDetailInfo(string gamePackageId);

        Task SoftRemoveGamePackage(string id);

        Task ActivateGamePackage(string id);
    }
}
