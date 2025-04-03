using BusinessObjects.Models;
using DAO;
using Repository.Repositories.GenericRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Repositories.UserRepositories
{
    public class UserRepository : GenericRepository<User>, IUserRepository
    {
        public UserRepository(InteractiveFloorManagementContext context) : base(context)
        {
        }

        public async Task<User> GetUserById(string userId)
        {
            return await GetSingle(u => u.Id.Equals(userId), includeProperties: "Role");
        }

        public async Task<User> GetUserByEmail(string email)
        {
            return await GetSingle(u => u.Email.ToLower().Equals(email.ToLower()), includeProperties: "Role");
        }
    }
}
