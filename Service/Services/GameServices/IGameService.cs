using BusinessObjects.DTOs.Game;

namespace Service.Services.GameServices
{
    public interface IGameService
    {
        Task<List<GameResponse>> GetAllAsync();
        Task<GameResponse?> GetByIdAsync(string id);
        Task<GameResponse> CreateAsync(CreateGameRequest request);
        Task<GameResponse> UpdateAsync(string id, UpdateGameRequest request);
        Task DeleteAsync(string id);
        Task UpdatePlayCount(string id);
        Task<String> GetDownloadUrl(string id);
        Task<GameResponse> AddVersionAsync(string gameId, AddGameVersionRequest request);
        Task<List<GameResponse>> GetPurchasedGamesByUserIdAsync(string userId);
    }
}