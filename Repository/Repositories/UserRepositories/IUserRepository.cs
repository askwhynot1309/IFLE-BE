using BusinessObjects.Models;
using Repository.Repositories.GenericRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Repositories.UserRepositories
{
    public interface IUserRepository : IGenericRepository<User>
    {
        Task<User> GetUserById(string userId);

        Task<User> GetUserByEmail(string email);

        Task<List<User>> GetCustomerList();

        Task<List<User>> GetCustomerListById(List<string> userIdList);

        Task<List<User>> GetStaffList();
    }
}
