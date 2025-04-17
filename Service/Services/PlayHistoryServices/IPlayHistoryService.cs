using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObjects.DTOs.PlayHistory;
using BusinessObjects.Models;
using Repository.Repositories.GenericRepositories;

namespace Service.Services.PlayHistoryServices
{
    public interface IPlayHistoryService
    {
        Task<PlayHistory> CreateAsync(PlayHistoryRequest request);
        Task<PlayHistory> GetHighScore(string userId, string gameId);
    }
}
