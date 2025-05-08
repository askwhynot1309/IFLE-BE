using BusinessObjects.Models;
using DAO;
using Microsoft.EntityFrameworkCore;
using Repository.Repositories.GenericRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Repositories.ActiveUserRepositories
{
    public class ActiveUserRepository : GenericRepository<ActiveUser>, IActiveUserRepository
    {
        private readonly InteractiveFloorManagementContext _context;
        public ActiveUserRepository(InteractiveFloorManagementContext context) : base(context)
        {
            _context = context;
        }

        public async Task<ActiveUser?> GetActiveUserByUserId(string userId)
        {
            return await context.ActiveUsers
                .FirstOrDefaultAsync(au => au.UserId == userId);
        }

        public async Task UpdateUserActiveStatus(string userId, bool isActive)
        {
            var activeUser = await GetActiveUserByUserId(userId);
            if (activeUser != null)
            {
                activeUser.IsActive = isActive;
                await Update(activeUser);
            }
        }

        public async Task<List<ActiveUser>> GetAllActiveUsers()
        {
            return await context.ActiveUsers
                .Where(au => au.IsActive)
                .Include(au => au.User)
                .ToListAsync();
        }
    }
} 