using DTO;

namespace Service.Services.GameServices
{
    public interface IGameService
    {
        Task<List<GameResponse>> GetAllAsync();
        Task<GameResponse?> GetByIdAsync(string id);
        Task<GameResponse> CreateAsync(CreateGameRequest request);
        Task<GameResponse> UpdateAsync(string id, UpdateGameRequest request);
        Task DeleteAsync(string id);
    }
}