using BusinessObjects.DTOs.User.Request;
using BusinessObjects.DTOs.User.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Services.UserServices
{
    public interface IUserService
    {
        Task<UserOwnInfoResponseModel> GetUserOwnInfo(string userId);

        Task UpdateUserOwnInformation(string userId, UserUpdateRequestModel model);

        Task ChangePassword(UserChangePasswordRequestModel model, string userId);
    }
}
