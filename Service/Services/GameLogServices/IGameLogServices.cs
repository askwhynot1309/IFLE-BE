using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObjects.DTOs.GameLog;

namespace Service.Services.GameLogServices
{
    public interface IGameLogService
    {
        Task<GameLogResponse> CreateGameLogAsync(CreateGameLog createGameLog);
        Task<IEnumerable<GameLogResponse>> GetAllGameLogsAsync();
        Task<IEnumerable<GameLogResponse>> GetGameLogsByGameIdAsync(string gameId);
    }
}
