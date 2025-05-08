using AutoMapper;
using BusinessObjects.DTOs.UserPackage.Request;
using BusinessObjects.DTOs.UserPackage.Response;
using BusinessObjects.Models;
using Repository.Enums;
using Repository.Repositories.UserPackageOrderRepositories;
using Repository.Repositories.UserPackageRepositories;
using Service.Ultis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Service.Services.UserPackageServices
{
    public class UserPackageService : IUserPackageService
    {
        private readonly IUserPackageRepository _userPackageRepository;
        private readonly IUserPackageOrderRepository _userPackageOrderRepository;
        private readonly IMapper _mapper;

        public UserPackageService(IUserPackageRepository userPackageRepository, IMapper mapper, IUserPackageOrderRepository userPackageOrderRepository)
        {
            _userPackageRepository = userPackageRepository;
            _mapper = mapper;
            _userPackageOrderRepository = userPackageOrderRepository;
        }

        public async Task AddUserPackage(UserPackageCreateUpdateRequestModel model)
        {
            var check = await _userPackageRepository.IsNameExisted(model.Name);

            if (check)
            {
                throw new CustomException("Tên gói người dùng này đã tồn tại.");
            }
            var allPackages = await _userPackageRepository.GetAllUserPackages();
            foreach (var package in allPackages)
            {
                if (package.Price == model.Price && package.UserLimit == model.UserLimit)
                {
                    throw new CustomException($"Đã tồn tại gói {package.Name} có cùng giá và số lượng người tăng lên.");
                }
            }

            var newUserPackage = _mapper.Map<UserPackage>(model);

            newUserPackage.Status = UserPackageEnums.Active.ToString();
            newUserPackage.Id = Guid.NewGuid().ToString();

            await _userPackageRepository.Insert(newUserPackage);
        }

        public async Task UpdateUserPackage(UserPackageCreateUpdateRequestModel model, string id)
        {
            var userPackage = await _userPackageRepository.GetUserPackageById(id);
            if (userPackage == null)
            {
                throw new CustomException("Không tìm thấy gói người dùng này.");
            }

            if (!userPackage.Name.ToLower().Equals(model.Name.ToLower()))
            {
                var check = await _userPackageRepository.IsNameExisted(model.Name);

                if (check)
                {
                    throw new CustomException("Tên gói người dùng này đã tồn tại.");
                }
            }

            var availableOrder = await _userPackageOrderRepository.GetAvailableOrderListByPackageId(id);
            if (availableOrder.Count > 0)
            {
                throw new CustomException("Gói này đang được sử dụng hoặc trong quá trình giao dịch với người dùng. Không thể cập nhật.");
            }

            var allPackages = await _userPackageRepository.GetAllUserPackages();
            allPackages.Remove(userPackage);
            foreach (var package in allPackages)
            {
                if (package.Price == model.Price && package.UserLimit == model.UserLimit)
                {
                    throw new CustomException($"Đã tồn tại gói {package.Name} có cùng giá và số lượng người tăng lên.");
                }
            }
            _mapper.Map(model, userPackage);
            await _userPackageRepository.Update(userPackage);
        }

        public async Task<List<UserPackageListResponseModel>> GetAllUserPackages()
        {
            var userPackage = await _userPackageRepository.GetAllUserPackages();
            var result = _mapper.Map<List<UserPackageListResponseModel>>(userPackage.OrderBy(u => u.Name));
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
