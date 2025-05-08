using BusinessObjects.Models;
using Repository.Repositories.GenericRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Repositories.ActiveUserRepositories
{
    public interface IActiveUserRepository : IGenericRepository<ActiveUser>
    {
        Task<ActiveUser?> GetActiveUserByUserId(string userId);
        Task UpdateUserActiveStatus(string userId, bool isActive);
        Task<List<ActiveUser>> GetAllActiveUsers();
    }
} 