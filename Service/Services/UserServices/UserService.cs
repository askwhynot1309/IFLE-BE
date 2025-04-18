using AutoMapper;
using BusinessObjects.DTOs.User.Request;
using BusinessObjects.DTOs.User.Response;
using BusinessObjects.Models;
using Microsoft.AspNetCore.Http;
using Repository.Enums;
using Repository.Repositories.RoleRepositories;
using Repository.Repositories.UserRepositories;
using Service.Services.AuthenticationServices;
using Service.Ultis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace Service.Services.UserServices
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IRoleRepository _roleRepository;
        private readonly IMapper _mapper;
        private readonly IAuthenticationService _authenticationService;

        public UserService(IUserRepository userRepository, IMapper mapper, IAuthenticationService authenticationService, IRoleRepository roleRepository)
        {
            _userRepository = userRepository;
            _mapper = mapper;
            _authenticationService = authenticationService;
            _roleRepository = roleRepository;
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

        public async Task<string> UpdateUserOwnInformation(string userId, InfoUpdateRequestModel model)
        {
            var user = await _userRepository.GetUserById(userId);
            if (user == null)
            {
                throw new CustomException("Id user không tồn tại");
            }
            _mapper.Map(model, user);
            await _userRepository.Update(user);

            var (access, refresh) = await _authenticationService.GenerateJWT(userId);
            return access;
        }

        public async Task<string> UpdateUserAvatar(string userId, string avatarUrl)
        {
            var user = await _userRepository.GetUserById(userId);
            if (user == null)
            {
                throw new CustomException("Id user không tồn tại");
            }
            user.AvatarUrl = avatarUrl;
            await _userRepository.Update(user);

            var (access, refresh) = await _authenticationService.GenerateJWT(userId);
            return access;
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

        public async Task<List<UserInfoResponeModel>> GetCustomerList()
        {
            var users = await _userRepository.GetCustomerList();
            var result = _mapper.Map<List<UserInfoResponeModel>>(users);
            return result;
        }

        public async Task DeactivateCustomerAccount(List<string> userIdList)
        {
            var customerList = await _userRepository.GetCustomerListById(userIdList);

            foreach (var customer in customerList)
            {
                customer.Status = AccountStatusEnums.Inactive.ToString();
            }

            await _userRepository.UpdateRange(customerList);
        }

        public async Task ActivateCustomerAccount(List<string> userIdList)
        {
            var customerList = await _userRepository.GetCustomerListById(userIdList);

            foreach (var customer in customerList)
            {
                customer.Status = AccountStatusEnums.Active.ToString();
            }

            await _userRepository.UpdateRange(customerList);
        }

        public async Task CreateStaffAccount(StaffCreateRequestModel model)
        {
            var newStaff = _mapper.Map<User>(model);

            var roleId = await _roleRepository.GetRoleIdByName(RoleEnums.Staff.ToString());
            newStaff.RoleId = roleId;

            newStaff.Id = Guid.NewGuid().ToString();
            newStaff.Status = AccountStatusEnums.Active.ToString();
            newStaff.IsVerified = true;
            newStaff.CreatedAt = DateTime.Now;

            var (salt, hash) = PasswordHasher.HashPassword(model.Password);
            newStaff.Salt = salt;
            newStaff.Password = hash;

            await _userRepository.Insert(newStaff);
        }

        public async Task<List<UserInfoResponeModel>> GetStaffList()
        {
            var staffs = await _userRepository.GetStaffList();
            var result = _mapper.Map<List<UserInfoResponeModel>>(staffs);
            return result;
        }

        public async Task DeleteStaffAccount(string staffId)
        {
            var staff = await _userRepository.GetUserById(staffId);
            if (staff == null)
            {
                throw new CustomException("Không tìm thấy tài khoản nhân viên.");
            }

            var roleId = await _roleRepository.GetRoleIdByName(RoleEnums.Staff.ToString());

            if (!staff.RoleId.Equals(roleId))
            {
                throw new CustomException("Tài khoản này không phải là Staff.");
            }

            staff.RefreshTokens.Clear();
            staff.OTP.Clear();

            await _userRepository.Delete(staff);
        }

        public async Task ActivateStaffAccount(List<string> staffIdList)
        {
            var staffList = await _userRepository.GetStaffListById(staffIdList);

            foreach (var staff in staffList)
            {
                staff.Status = AccountStatusEnums.Active.ToString();
            }

            await _userRepository.UpdateRange(staffList);
        }

        public async Task DeactivateStaffAccount(List<string> staffIdList)
        {
            var staffList = await _userRepository.GetStaffListById(staffIdList);

            foreach (var staff in staffList)
            {
                staff.Status = AccountStatusEnums.Inactive.ToString();
            }

            await _userRepository.UpdateRange(staffList);
        }
    }
}
