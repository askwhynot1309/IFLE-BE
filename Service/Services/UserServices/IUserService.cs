﻿using BusinessObjects.DTOs.Email.Request;
using BusinessObjects.DTOs.Transaction.Response;
using BusinessObjects.DTOs.User.Request;
using BusinessObjects.DTOs.User.Response;
using BusinessObjects.Models;
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

        Task<string> UpdateUserOwnInformation(string userId, InfoUpdateRequestModel model);

        Task<string> UpdateUserAvatar(string userId, string avatarUrl);

        Task ChangePassword(UserChangePasswordRequestModel model, string userId);

        Task<List<UserInfoResponeModel>> GetCustomerList();

        Task DeactivateCustomerAccount(List<string> userIdList);

        Task CreateStaffAccount(StaffCreateRequestModel model);

        Task<List<UserInfoResponeModel>> GetStaffList();

        Task ActivateCustomerAccount(List<string> userIdList);

        Task ActivateStaffAccount(List<string> staffIdList);

        Task DeactivateStaffAccount(List<string> staffIdList);

        Task DeleteStaffAccount(string staffId);

        Task<List<TransactionResponseModel>> ViewOwnTransactions(string currentUserId);

        Task<List<TransactionResponseModel>> GetAllOrders();

        Task SendEmailFeedback(SendFeedbackRequestModel model);
    }
}
