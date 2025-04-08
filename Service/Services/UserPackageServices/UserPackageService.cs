using AutoMapper;
using BusinessObjects.DTOs.UserPackage.Request;
using BusinessObjects.DTOs.UserPackage.Response;
using BusinessObjects.Models;
using Repository.Enums;
using Repository.Repositories.UserPackageRepositories;
using Service.Ultis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Services.UserPackageServices
{
    public class UserPackageService : IUserPackageService
    {
        private readonly IUserPackageRepository _userPackageRepository;
        private readonly IMapper _mapper;

        public UserPackageService(IUserPackageRepository userPackageRepository, IMapper mapper)
        {
            _userPackageRepository = userPackageRepository;
            _mapper = mapper;
        }

        public async Task AddUserPackage(UserPackageCreateRequestModel model)
        {
            var newUserPackage = _mapper.Map<UserPackage>(model);

            newUserPackage.Status = UserPackageEnums.Active.ToString();
            newUserPackage.Id = Guid.NewGuid().ToString();

            await _userPackageRepository.Insert(newUserPackage);
        }

        public async Task<List<UserPackageListResponseModel>> GetAllUserPackages()
        {
            var userPackage = await _userPackageRepository.GetAllUserPackages();
            var result = _mapper.Map<List<UserPackageListResponseModel>>(userPackage);
            return result;
        }

        public async Task<List<UserPackageListResponseModel>> GetActiveUserPackages()
        {
            var userPackage = await _userPackageRepository.GetActiveUserPackages();
            var result = _mapper.Map<List<UserPackageListResponseModel>>(userPackage);
            return result;
        }

        public async Task SoftRemoveUserPackage(string id)
        {
            var userPackage = await _userPackageRepository.GetUserPackageById(id);

            if (userPackage == null)
            {
                throw new CustomException("Không tìm thấy gói nguời dùng này.");
            }

            userPackage.Status = UserPackageEnums.Inactive.ToString();

            await _userPackageRepository.Update(userPackage);
        }

        public async Task ActivateUserPackage(string id)
        {
            var userPackage = await _userPackageRepository.GetUserPackageById(id);

            if (userPackage == null)
            {
                throw new CustomException("Không tìm thấy gói nguời dùng này.");
            }

            userPackage.Status = UserPackageEnums.Active.ToString();

            await _userPackageRepository.Update(userPackage);
        }
    }
}
