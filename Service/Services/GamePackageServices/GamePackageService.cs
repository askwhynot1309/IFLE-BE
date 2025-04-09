using AutoMapper;
using BusinessObjects.DTOs.GamePackage.Request;
using BusinessObjects.DTOs.GamePackage.Response;
using BusinessObjects.Models;
using Repository.Enums;
using Repository.Repositories.GamePackageRelationRepositories;
using Repository.Repositories.GamePackageRepositories;
using Service.Ultis;
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
        private readonly IGamePackageRelationRepository _gamePackageRelationRepository;
        private readonly IMapper _mapper;

        public GamePackageService(IGamePackageRepository gamePackageRepository, IMapper mapper, IGamePackageRelationRepository gamePackageRelationRepository)
        {
            _gamePackageRepository = gamePackageRepository;
            _mapper = mapper;
            _gamePackageRelationRepository = gamePackageRelationRepository;
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
            var gamePackage = await _gamePackageRepository.GetGamePackageById(packageId);
            if (gamePackage == null)
            {
                throw new CustomException("Không tìm thấy gói game.");
            }

            var existedGameId = await _gamePackageRelationRepository.GetListGameIdByGamePackageId(packageId);
            var list = existedGameId.Intersect(gameIdList);
            if (list.Count() > 0)
            {
                throw new CustomException("Game này đã được thêm vào trước đó.");
            }

            var newGamePackageRelationList = new List<GamePackageRelation>();
            foreach (var gameId in gameIdList)
            {
                newGamePackageRelationList.Add(new GamePackageRelation
                {
                    Id = Guid.NewGuid().ToString(),
                    GameId = gameId,
                    GamePackageId = packageId
                });
            }
            await _gamePackageRelationRepository.InsertRange(newGamePackageRelationList);
        }

        public async Task<GamePackageDetailsResponseModel> GetGamePackageDetailInfo(string gamePackageId)
        {
            var gamePackage = await _gamePackageRepository.GetGamePackageById(gamePackageId);

            var listGameRelation = await _gamePackageRelationRepository.GetListByGamePackageId(gamePackageId);
            var listGame = listGameRelation.Select(l => l.Game);

            return new GamePackageDetailsResponseModel
            {
                Id = gamePackage.Id,
                Name = gamePackage.Name,
                Description = gamePackage.Description,
                Duration = gamePackage.Duration,
                Price = gamePackage.Price,
                Status = gamePackage.Status,
                GameList = _mapper.Map<List<GameInfo>>(listGame)
            };
        }

        public async Task SoftRemoveGamePackage(string id)
        {
            var gamePackage = await _gamePackageRepository.GetGamePackageById(id);

            if (gamePackage == null)
            {
                throw new CustomException("Không tìm thấy gói game này.");
            }

            gamePackage.Status = GamePackageEnums.Inactive.ToString();

            await _gamePackageRepository.Update(gamePackage);
        }

        public async Task ActivateGamePackage(string id)
        {
            var gamePackage = await _gamePackageRepository.GetGamePackageById(id);
            if (gamePackage == null)
            {
                throw new CustomException("Không tìm thấy gói game này.");
            }
            gamePackage.Status = GamePackageEnums.Active.ToString();

            await _gamePackageRepository.Update(gamePackage);
        }
    }
}
