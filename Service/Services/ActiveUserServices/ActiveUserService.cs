using BusinessObjects.Models;
using Repository.Repositories.ActiveUserRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Services.ActiveUserServices
{
    public class ActiveUserService : IActiveUserService
    {
        private readonly IActiveUserRepository _activeUserRepository;

        public ActiveUserService(IActiveUserRepository activeUserRepository)
        {
            _activeUserRepository = activeUserRepository;
        }

        public async Task<ActiveUser> TrackUserLogin(string userId)
        {
            var existingActivity = await _activeUserRepository.GetActiveUserByUserId(userId);
            if (existingActivity != null)
            {
                existingActivity.LoginTime = DateTime.Now;
                existingActivity.IsActive = true;
                await _activeUserRepository.Update(existingActivity);
                return existingActivity;
            }

            var newActivity = new ActiveUser
            {
                Id = Guid.NewGuid().ToString(),
                UserId = userId,
                LoginTime = DateTime.Now,
                IsActive = true
            };

            await _activeUserRepository.Insert(newActivity);
            return newActivity;
        }

        public async Task UpdateUserActiveStatus(string userId, bool isActive)
        {
            await _activeUserRepository.UpdateUserActiveStatus(userId, isActive);
        }

        public async Task<bool> IsUserActive(string userId)
        {
            var activeUser = await _activeUserRepository.GetActiveUserByUserId(userId);
            return activeUser?.IsActive ?? false;
        }

        public async Task<List<ActiveUser>> GetAllActiveUsers()
        {
            return await _activeUserRepository.GetAllActiveUsers();
        }

        public async Task<ActiveUser> GetActiveUserById(string userId)
        {
            return await _activeUserRepository.GetActiveUserByUserId(userId);
        }
    }
} 