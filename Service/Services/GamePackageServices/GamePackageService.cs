using AutoMapper;
using BusinessObjects.DTOs.GamePackage.Request;
using BusinessObjects.DTOs.GamePackage.Response;
using BusinessObjects.Models;
using Repository.Enums;
using Repository.Repositories.GamePackageRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Services.GamePackageServices
{
    public class GamePackageService : IGamePackageService
    {
        private readonly IGamePackageRepository _gamePackageRepository;
        private readonly IMapper _mapper;

        public GamePackageService(IGamePackageRepository gamePackageRepository, IMapper mapper)
        {
            _gamePackageRepository = gamePackageRepository;
            _mapper = mapper;
        }

        public async Task CreateGamePackage(GamePackageCreateRequestModel model)
        {
            var newGamePackage = _mapper.Map<GamePackage>(model);

            newGamePackage.Id = Guid.NewGuid().ToString();
            newGamePackage.Status = GamePackageEnums.Active.ToString();

            await _gamePackageRepository.Insert(newGamePackage);
        }

        public async Task<List<GamePackageListResponseModel>> GetAllGamePackages()
        {
            var gamePackages = await _gamePackageRepository.GetAllGamePackages();   
            var result = _mapper.Map<List<GamePackageListResponseModel>>(gamePackages);
            return result;
        }

        public async Task<List<GamePackageListResponseModel>> GetActiveGamePackages()
        {
            var gamePackages = await _gamePackageRepository.GetActiveGamePackages();
            var result = _mapper.Map<List<GamePackageListResponseModel>>(gamePackages);
            return result;
        }

        public async Task AddGameToPackage(string packageId, List<string> gameIdList)
        {
            
        }
    }
}
