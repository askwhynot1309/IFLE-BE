using BusinessObjects.DTOs.UserPackage.Request;
using BusinessObjects.DTOs.UserPackage.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Services.UserPackageServices
{
    public interface IUserPackageService
    {
        Task AddUserPackage(UserPackageCreateUpdateRequestModel model);

        Task UpdateUserPackage(UserPackageCreateUpdateRequestModel model, string id);

        Task<List<UserPackageListResponseModel>> GetAllUserPackages();

        Task<List<UserPackageListResponseModel>> GetActiveUserPackages();

        Task SoftRemoveUserPackage(string id);

        Task ActivateUserPackage(string id);
    }
}
