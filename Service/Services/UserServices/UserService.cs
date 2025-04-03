using AutoMapper;
using BusinessObjects.DTOs.User.Request;
using BusinessObjects.DTOs.User.Response;
using Repository.Repositories.UserRepositories;
using Service.Ultis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Services.UserServices
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public UserService(IUserRepository userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public async Task<UserOwnInfoResponseModel> GetUserOwnInfo(string userId)
        {
            var user = await _userRepository.GetUserById(userId);
            if (user == null)
            {
                throw new CustomException("Id user không tồn tại");
            }
            return _mapper.Map<UserOwnInfoResponseModel>(user);
        }

        public async Task UpdateUserOwnInformation(string userId, UserUpdateRequestModel model)
        {
            var user = await _userRepository.GetUserById(userId);
            if (user == null)
            {
                throw new CustomException("Id user không tồn tại");
            }
            _mapper.Map(model, user);
            await _userRepository.Update(user);
        }

        public async Task ChangePassword(UserChangePasswordRequestModel model, string userId)
        {
            var user = await _userRepository.GetUserById(userId);
            if (user == null)
            {
                throw new CustomException("Tài khoản không tồn tại");
            }

            if (!PasswordHasher.VerifyPassword(model.OldPassword, user.Salt, user.Password))
            {
                throw new CustomException("Mật khẩu bạn nhập không chính xác");
            }

            if (PasswordHasher.VerifyPassword(model.NewPassword, user.Salt, user.Password))
            {
                throw new CustomException("Mật khẩu mới bạn nhập không được giống mật khẩu cũ.");
            }

            var (salt, hash) = PasswordHasher.HashPassword(model.NewPassword);
            user.Password = hash;
            user.Salt = salt;

            await _userRepository.Update(user);
        }
    }
}
