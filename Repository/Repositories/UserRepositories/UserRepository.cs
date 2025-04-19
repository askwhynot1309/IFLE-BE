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
            return await GetSingle(u => u.Id.Equals(userId), includeProperties: "Role,OrganizationUsers,RefreshTokens,OTP");
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

        public async Task<List<User>> GetCustomerListByIdList(List<string> userIdList)
        {
            var userList = await Get(u => userIdList.Contains(u.Id), includeProperties: "OrganizationUsers");
            return userList.ToList();
        }

        public async Task<List<User>> GetStaffList()
        {
            var staffs = await Get(u => u.Role.Name.Equals(RoleEnums.Staff.ToString()));
            return staffs.ToList();
        }

        public async Task<List<string>> GetUserIdListByEmailList(List<string> emailList)
        {
            var lowerEmailList = emailList.Select(email => email.ToLower());

            var userList = await Get(u => lowerEmailList.Contains(u.Email.ToLower()));

            var userIdList = userList.Select(user => user.Id);
            return userIdList.ToList();
        }

        public async Task<List<User>> GetStaffListById(List<string> staffIdList)
        {
            var staffList = await Get(u => staffIdList.Contains(u.Id));
            return staffList.ToList();
        }

        public async Task<List<User>> GetUserListByEmailList(List<string> emailList)
        {
            var lowerEmailList = emailList.Select(email => email.ToLower());
            var userList = await Get(u => lowerEmailList.Contains(u.Email.ToLower()));
            return userList.ToList();
        }
    }
}
