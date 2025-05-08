using AutoMapper;
using BusinessObjects.DTOs.GamePackage.Request;
using BusinessObjects.DTOs.GamePackage.Response;
using BusinessObjects.Models;
using Repository.Enums;
using Repository.Repositories.GamePackageOrderRepositories;
using Repository.Repositories.GamePackageRelationRepositories;
using Repository.Repositories.GamePackageRepositories;
using Repository.Repositories.GameRepositories;
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
        private readonly IGamePackageOrderRepository _gamePackageOrderRepository;
        private readonly IGameRepository _gameRepository;

        public GamePackageService(IGamePackageRepository gamePackageRepository, IMapper mapper, IGamePackageRelationRepository gamePackageRelationRepository, IGamePackageOrderRepository gamePackageOrderRepository, IGameRepository gameRepository)
        {
            _gamePackageRepository = gamePackageRepository;
            _mapper = mapper;
            _gamePackageRelationRepository = gamePackageRelationRepository;
            _gamePackageOrderRepository = gamePackageOrderRepository;
            _gameRepository = gameRepository;
        }

        public async Task CreateGamePackage(GamePackageCreateRequestModel model)
        {
            var check = await _gamePackageRepository.IsNameExisted(model.Name);
            if (check)
            {
                throw new CustomException("Tên gói trò chơi này đã tồn tại.");
            }

            if (model.GameIdList.Count == 0)
            {
                throw new CustomException("Vui lòng chọn game để thêm vào gói.");
            }

            var listGame = await _gameRepository.GetListGameByListId(model.GameIdList);
            if (listGame.Count != model.GameIdList.Count)
            {
                throw new CustomException("Không tìm thấy game bạn thêm vào. Vui lòng chọn đúng game có sẵn.");
            }

            var allPackages = await _gamePackageRepository.GetAllGamePackages();
            foreach (var package in allPackages)
            {
                if (package.Price == model.Price && package.Duration == model.Duration)
                {
                    var relatedGames = await _gamePackageRelationRepository.GetListGameIdByGamePackageId(package.Id);
                    if (relatedGames.Count == model.GameIdList.Count &&
                        !relatedGames.Except(model.GameIdList).Any())
                    {
                        throw new CustomException($"Đã tồn tại gói {package.Name} có cùng giá, thời hạn và danh sách game.");
                    }
                }
            }

            var newGamePackage = _mapper.Map<GamePackage>(model);
            newGamePackage.Id = Guid.NewGuid().ToString();
            newGamePackage.Status = GamePackageEnums.Active.ToString();

            await _gamePackageRepository.Insert(newGamePackage);

            var list = new List<GamePackageRelation>();
            foreach (var gameId in model.GameIdList)
            {
                list.Add(new GamePackageRelation
                {
                    Id = Guid.NewGuid().ToString(),
                    GameId = gameId,
                    GamePackageId = newGamePackage.Id,
                });
            }

            await _gamePackageRelationRepository.InsertRange(list);
        }


        public async Task UpdateGamePackage(GamePackageUpdateRequestModel model, string gamePackageId)
        {
            var listAvailableOrder = await _gamePackageOrderRepository.GetAvailableOrderListByPackageId(gamePackageId);
            if (listAvailableOrder.Count > 0)
            {
                throw new CustomException("Gói trò chơi này đang được sử dụng hoặc trong quá trình giao dịch với người dùng. Không thể cập nhật gói trò chơi này.");
            }

            var gamePackage = await _gamePackageRepository.GetGamePackageById(gamePackageId);
            if (gamePackage != null && !gamePackage.Name.ToLower().Equals(model.Name.ToLower()))
            {
                var check = await _gamePackageRepository.IsNameExisted(model.Name);

                if (check)
                {
                    throw new CustomException("Tên gói trò chơi này đã tồn tại.");
                }
            }
            var allPackages = await _gamePackageRepository.GetAllGamePackages();
            allPackages.Remove(gamePackage);
            foreach (var package in allPackages)
            {
                if (package.Price == model.Price && package.Duration == model.Duration)
                {
                    var relatedGames = await _gamePackageRelationRepository.GetListGameIdByGamePackageId(package.Id);
                    if (relatedGames.Count == model.GameIdList.Count &&
                        !relatedGames.Except(model.GameIdList).Any())
                    {
                        throw new CustomException($"Đã tồn tại gói {package.Name} có cùng giá, thời hạn và danh sách game.");
                    }
                }
            }
            _mapper.Map(model, gamePackage);

            if (model.GameIdList.Count > 0)
            {
                gamePackage.GamePackageRelations.Clear();
                foreach (var gameId in model.GameIdList)
                {
                    gamePackage.GamePackageRelations.Add(new GamePackageRelation
                    {
                        Id = Guid.NewGuid().ToString(),
                        GameId = gameId,
                        GamePackageId = gamePackageId
                    });
                }
            }

            await _gamePackageRepository.Update(gamePackage);
        }

        public async Task<List<GamePackageListResponseModel>> GetAllGamePackages()
        {
            var gamePackages = await _gamePackageRepository.GetAllGamePackages();
            var result = gamePackages.Select(g => new GamePackageListResponseModel
            {
                Id = g.Id,
                Name = g.Name,
                Description = g.Description,
                Duration = g.Duration,
                Price = g.Price,
                Status = g.Status,
                GameList = _mapper.Map<List<GameInfo>>(g.GamePackageRelations.Select(g => g.Game))
            }).OrderBy(o => o.Name).ToList();
            return result;
        }

        public async Task<List<GamePackageListResponseModel>> GetActiveGamePackages()
        {
            var gamePackages = await _gamePackageRepository.GetActiveGamePackages();
            var result = gamePackages.Select(g => new GamePackageListResponseModel
            {
                Id = g.Id,
                Name = g.Name,
                Description = g.Description,
                Duration = g.Duration,
                Price = g.Price,
                Status = g.Status,
                GameList = _mapper.Map<List<GameInfo>>(g.GamePackageRelations.Select(g => g.Game))
            }).OrderBy(o => o.Name).ToList();
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
