using BusinessObjects.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Services.ActiveUserServices
{
    public interface IActiveUserService
    {
        Task<ActiveUser> TrackUserLogin(string userId);
        Task UpdateUserActiveStatus(string userId, bool isActive);
        Task<bool> IsUserActive(string userId);
        Task<List<ActiveUser>> GetAllActiveUsers();
        Task<ActiveUser?> GetActiveUserById(string userId);
    }
} 