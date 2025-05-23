﻿using BusinessObjects.DTOs.User.Request;
using BusinessObjects.DTOs.User.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Services.AuthenticationServices
{
    public interface IAuthenticationService
    {
        Task<(string accessToken, string refreshToken)> GenerateJWT(string userId);

        Task RegisterWithPassword(UserRegisterWithPwRequestModel request);

        Task<UserAuthResponseModel> LoginAuthenticate(UserLoginRequestModel request);
        Task<UserAuthResponseModel> LoginAuthenticateDesktopApp(UserLoginRequestModel request);
        Task<UserAuthResponseModel> CheckRefreshToken(string refreshToken);
    }
}
