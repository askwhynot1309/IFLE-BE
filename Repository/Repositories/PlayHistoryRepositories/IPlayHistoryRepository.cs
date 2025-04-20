using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObjects.Models;
using Repository.Repositories.GenericRepositories;

namespace Repository.Repositories.PlayHistoryRepositories
{
    public interface IPlayHistoryRepository: IGenericRepository<PlayHistory>
    {
        Task<PlayHistory> CreateAsync(PlayHistory playHistory);
        Task<PlayHistory> GetHighScore(string userId, string gameId);
        Task<List<PlayHistory>> GetFloorPlayHistory(string floorId);
    }
}
