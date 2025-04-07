using BusinessObjects.Models;
using DAO;
using Repository.Enums;
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
            return await GetSingle(u => u.Id.Equals(userId), includeProperties: "Role,OrganizationUsers");
        }

        public async Task<User> GetUserByEmail(string email)
        {
            return await GetSingle(u => u.Email.ToLower().Equals(email.ToLower()), includeProperties: "Role");
        }

        public async Task<List<User>> GetCustomerList()
        {
            var users = await Get(u => u.Role.Name.Equals(RoleEnums.Customer.ToString()));
            return users.ToList();
        }

        public async Task<List<User>> GetCustomerListById(List<string> userIdList)
        {
            var userList = await Get(u => userIdList.Contains(u.Id));
            return userList.ToList();
        }

        public async Task<List<User>> GetStaffList()
        {
            var staffs = await Get(u => u.Role.Name.Equals(RoleEnums.Staff.ToString()));
            return staffs.ToList();
        }

        public async Task<List<User>> GetUsersByIdList(List<string> userIdList)
        {
            var users = await Get(u => userIdList.Contains(u.Id), includeProperties: "OrganizationUsers");
            return users.ToList();
        }
    }
}
