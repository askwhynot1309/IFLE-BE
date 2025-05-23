﻿using AutoMapper;
using BusinessObjects.DTOs.Email.Request;
using BusinessObjects.DTOs.Transaction.Response;
using BusinessObjects.DTOs.User.Request;
using BusinessObjects.DTOs.User.Response;
using BusinessObjects.Models;
using Microsoft.AspNetCore.Http;
using Repository.Enums;
using Repository.Repositories.GamePackageOrderRepositories;
using Repository.Repositories.RoleRepositories;
using Repository.Repositories.UserPackageOrderRepositories;
using Repository.Repositories.UserRepositories;
using Service.Services.AuthenticationServices;
using Service.Services.EmailServices;
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
        private readonly IGamePackageOrderRepository _gamePackageOrderRepository;
        private readonly IUserPackageOrderRepository _userPackageOrderRepository;
        private readonly IEmailService _emailService;

        public UserService(IUserRepository userRepository, IMapper mapper, IAuthenticationService authenticationService, IRoleRepository roleRepository, IGamePackageOrderRepository gamePackageOrderRepository, IUserPackageOrderRepository userPackageOrderRepository, IEmailService emailService)
        {
            _userRepository = userRepository;
            _mapper = mapper;
            _authenticationService = authenticationService;
            _roleRepository = roleRepository;
            _gamePackageOrderRepository = gamePackageOrderRepository;
            _userPackageOrderRepository = userPackageOrderRepository;
            _emailService = emailService;
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
            var result = _mapper.Map<List<UserInfoResponeModel>>(users.OrderBy(u => u.FullName));
            return result;
        }

        public async Task DeactivateCustomerAccount(List<string> userIdList)
        {
            var customerList = await _userRepository.GetCustomerListByIdList(userIdList);

            foreach (var customer in customerList)
            {
                customer.Status = AccountStatusEnums.Inactive.ToString();
            }

            await _userRepository.UpdateRange(customerList);
        }

        public async Task ActivateCustomerAccount(List<string> userIdList)
        {
            var customerList = await _userRepository.GetCustomerListByIdList(userIdList);

            foreach (var customer in customerList)
            {
                customer.Status = AccountStatusEnums.Active.ToString();
            }

            await _userRepository.UpdateRange(customerList);
        }

        public async Task CreateStaffAccount(StaffCreateRequestModel model)
        {
            var existed = await _userRepository.GetUserByEmail(model.Email);
            if (existed != null)
            {
                throw new CustomException("Tài khoản với email này đã tồn tại, không thể tạo thêm.");
            }
            var newStaff = _mapper.Map<User>(model);

            var roleId = await _roleRepository.GetRoleIdByName(RoleEnums.Staff.ToString());
            newStaff.RoleId = roleId;

            newStaff.Id = Guid.NewGuid().ToString();
            newStaff.Status = AccountStatusEnums.Active.ToString();
            newStaff.IsVerified = true;
            newStaff.CreatedAt = DateTime.Now;

            var randomPassword = PasswordHasher.GenerateRandomPassword();

            var (salt, hash) = PasswordHasher.HashPassword(randomPassword);
            newStaff.Salt = salt;
            newStaff.Password = hash;

            var htmlBody = HTMLEmailTemplate.StaffAccountCreatedNotification(model.Email, model.FullName, randomPassword);
            bool sendEmailSuccess = await _emailService.SendEmail(model.Email, "Phản hồi từ người dùng", htmlBody);
            if (!sendEmailSuccess)
            {
                throw new CustomException("Đã xảy ra lỗi trong quá trình gửi email.");
            }

            await _userRepository.Insert(newStaff);
        }

        public async Task<List<UserInfoResponeModel>> GetStaffList()
        {
            var staffs = await _userRepository.GetStaffList();
            var result = _mapper.Map<List<UserInfoResponeModel>>(staffs.OrderBy(s => s.FullName));
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

        public async Task<List<TransactionResponseModel>> ViewOwnTransactions(string currentUserId)
        {
            var listG = await _gamePackageOrderRepository.GetOwnGamePackageOrder(currentUserId);
            var listU = await _userPackageOrderRepository.GetOwnUserPackageOrder(currentUserId);

            var resultG = _mapper.Map<List<TransactionResponseModel>>(listG);
            var resultU = _mapper.Map<List<TransactionResponseModel>>(listU);

            foreach (var transaction in resultG)
            {
                transaction.PackageCategory = "GamePackage";
            }

            foreach (var transaction in resultU)
            {
                transaction.PackageCategory = "UserPackage";
            }

            resultG = resultG.Concat(resultU).ToList();
            return resultG;
        }

        public async Task<List<TransactionResponseModel>> GetAllOrders()
        {
            var userOrders = await _userPackageOrderRepository.GetAllOrders();
            var gameOrders = await _gamePackageOrderRepository.GetAllOrders();

            var userOrder1 = userOrders.Select(x => new TransactionResponseModel
            {
                Id = x.Id,
                Price = x.Price,
                OrderDate = x.OrderDate,
                OrderCode = x.OrderCode,
                PaymentMethod = x.PaymentMethod,
                Status = x.Status,
                PackageName = x.UserPackage.Name,
                PackageCategory = "User Package",
                UserInfo = _mapper.Map<UserInfoResponeModel>(x.User)
            });

            var gameOrder2 = gameOrders.Select(x => new TransactionResponseModel
            {
                Id = x.Id.ToString(),
                Price = x.Price,
                OrderDate = x.OrderDate,
                OrderCode = x.OrderCode,
                PackageName = x.GamePackage.Name,
                PaymentMethod = x.PaymentMethod,
                Status = x.Status,
                PackageCategory = "Game Package",
                UserInfo = _mapper.Map<UserInfoResponeModel>(x.User)
            });

            return userOrder1.Concat(gameOrder2).OrderByDescending(x => x.OrderDate).ToList();
        }

        public async Task SendEmailFeedback(SendFeedbackRequestModel model)
        {
            var htmlBody = HTMLEmailTemplate.SendingFeedback(model.Name, model.Email, model.Content);
            bool sendEmailSuccess = await _emailService.SendEmail("interactivefloor.ifle@gmail.com", "Phản hồi từ người dùng", htmlBody);
            if (!sendEmailSuccess)
            {
                throw new CustomException("Đã xảy ra lỗi trong quá trình gửi email.");
            }
        }
    }
}
