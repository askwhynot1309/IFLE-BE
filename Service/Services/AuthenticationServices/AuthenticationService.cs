using AutoMapper;
using BusinessObjects.DTOs.User.Request;
using BusinessObjects.DTOs.User.Response;
using BusinessObjects.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Repository.Enums;
using Repository.Repositories.OTPRepositories;
using Repository.Repositories.RefreshTokenRepositories;
using Repository.Repositories.RoleRepositories;
using Repository.Repositories.UserRepositories;
using Service.Services.EmailServices;
using Service.Ultis;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Service.Services.AuthenticationServices
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly IUserRepository _userRepository;
        private readonly IRoleRepository _roleRepository;
        private readonly IRefreshTokenRepository _refreshTokenRepository;
        private readonly IConfiguration _config;
        private readonly JwtSecurityTokenHandler _tokenHandler;
        private readonly IMapper _mapper;
        private readonly IEmailService _emailService;
        private readonly IOTPRepository _oTPRepository;

        public AuthenticationService(IUserRepository userRepository, IConfiguration configuration, IMapper mapper, IEmailService emailService, IOTPRepository oTPRepository, IRoleRepository roleRepository, IRefreshTokenRepository refreshTokenRepository)
        {
            _userRepository = userRepository;
            _config = configuration;
            _mapper = mapper;
            _tokenHandler = new JwtSecurityTokenHandler();
            _emailService = emailService;
            _oTPRepository = oTPRepository;
            _roleRepository = roleRepository;
            _refreshTokenRepository = refreshTokenRepository;
        }

        public async Task<(string accessToken, string refreshToken)> GenerateJWT(string userId)
        {
            var user = await _userRepository.GetUserById(userId);
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credential = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            List<Claim> accessClaims = new()
            {
                new Claim(ClaimsIdentity.DefaultRoleClaimType, user.Role.Name),
                new Claim("userId", user.Id),
                new Claim("fullname", user.FullName),
                new Claim("email", user.Email),
                new Claim("role", user.Role.Name),
                new Claim("avatarUrl", user.AvatarUrl != null ? user.AvatarUrl : string.Empty),
            };

            List<Claim> refreshClaims = new()
            {
                new Claim("userId", user.Id),
            };

            var access = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: accessClaims,
                expires: DateTime.Now.AddHours(1),
                signingCredentials: credential
                );

            var refresh = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: refreshClaims,
                expires: DateTime.Now.AddDays(7),
                signingCredentials: credential
                );

            var accesstoken = _tokenHandler.WriteToken(access);
            var refreshToken = _tokenHandler.WriteToken(refresh);

            return (accesstoken, refreshToken);
        }

        private string CreateNewOTPCode()
        {
            return new Random().Next(0, 999999).ToString("D6");
        }

        public async Task RegisterWithPassword(UserRegisterWithPwRequestModel request)
        {
            User currentUser = await _userRepository.GetUserByEmail(request.Email);

            if (currentUser != null)
            {
                throw new CustomException("Email bạn nhập đã kết nối với tài khoản khác.");
            }

            User newUser = _mapper.Map<User>(request);
            newUser.Id = Guid.NewGuid().ToString();
            var customerRoleId = await _roleRepository.GetRoleIdByName(nameof(RoleEnums.Customer));
            newUser.RoleId = customerRoleId;
            newUser.Status = AccountStatusEnums.Active.ToString();
            newUser.CreatedAt = DateTime.Now;
            newUser.IsVerified = false;
            var (salt, hash) = PasswordHasher.HashPassword(request.InputPassword);
            newUser.Password = hash;
            newUser.Salt = salt;
            var newOTP = CreateNewOTPCode();
            var htmlBody = HTMLEmailTemplate.VerifyEmailOTP(request.Fullname, newOTP);
            bool sendEmailSuccess = await _emailService.SendEmail(request.Email, "Xác thực email", htmlBody);
            if (!sendEmailSuccess)
            {
                throw new CustomException("Đã xảy ra lỗi trong quá trình gửi email.");
            }

            OTP newOTPCode = new OTP()
            {
                Id = Guid.NewGuid().ToString(),
                Code = newOTP,
                UserId = newUser.Id,
                CreatedAt = DateTime.Now,
                IsUsed = false,
            };

            await _userRepository.Insert(newUser);
            await _oTPRepository.Insert(newOTPCode);
        }

        public async Task<UserAuthResponseModel> LoginAuthenticate(UserLoginRequestModel request)
        {
            var user = await _userRepository.GetUserByEmail(request.Email);
            if (user != null)
            {
                if (!PasswordHasher.VerifyPassword(request.Password, user.Salt, user.Password))
                {
                    throw new CustomException("Mật khẩu bạn vừa nhập chưa chính xác.");
                }

                if (!user.IsVerified)
                {
                    throw new CustomException("Tài khoản của bạn chưa được xác thực email!", StatusCodes.Status403Forbidden);
                }

                if (user.Status.Equals(AccountStatusEnums.Inactive.ToString()))
                {
                    throw new CustomException("Tài khoản của bạn đã vi phạm và bị cấm khỏi hệ thống hoặc chưa trải qua xác thực từ chúng tôi.");
                }

                var (accessToken, refreshToken) = await GenerateJWT(user.Id);

                var existRefreshToken = await _refreshTokenRepository.GetRefreshTokenByUserId(user.Id);
                if (existRefreshToken != null)
                {
                    existRefreshToken.Token = refreshToken;
                    existRefreshToken.ExpiredAt = DateTime.Now.AddDays(7);

                    await _refreshTokenRepository.Update(existRefreshToken);
                }
                else
                {
                    var newRefreshToken = new RefreshToken()
                    {
                        Id = Guid.NewGuid().ToString(),
                        Token = refreshToken,
                        ExpiredAt = DateTime.Now.AddDays(7),
                        UserId = user.Id
                    };

                    await _refreshTokenRepository.Insert(newRefreshToken);
                }

                return new UserAuthResponseModel()
                {
                    AccessToken = accessToken,
                    RefreshToken = refreshToken,
                };
            }
            else
            {
                throw new CustomException("Email bạn nhập không kết nối với tài khoản nào.");
            }
        }

        public ClaimsPrincipal? ValidateRefreshToken(string refreshToken)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_config["Jwt:Key"]);

            var validationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = _config["Jwt:Issuer"],
                ValidAudience = _config["Jwt:Audience"],
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ClockSkew = TimeSpan.Zero
            };

            try
            {
                var principal = tokenHandler.ValidateToken(refreshToken, validationParameters, out SecurityToken validatedToken);
                return principal;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<UserAuthResponseModel> CheckRefreshToken(string refreshToken)
        {
            var principal = ValidateRefreshToken(refreshToken);
            if (principal == null)
            {
                throw new CustomException("Invalid refresh token");
            }

            var userId = principal.FindFirst("userId")?.Value;
            if (userId == null)
            {
                throw new CustomException("Invalid token claims");
            }

            var oldRefreshToken = await _refreshTokenRepository.GetRefreshTokenByToken(refreshToken);
            if (oldRefreshToken == null || !userId.Equals(oldRefreshToken.UserId))
            {
                throw new CustomException("Invalid refresh token");
            }

            var (newAccessToken, newRefreshToken) = await GenerateJWT(userId);

            oldRefreshToken.Token = newRefreshToken;
            oldRefreshToken.ExpiredAt = DateTime.Now.AddDays(7);
            await _refreshTokenRepository.Update(oldRefreshToken);

            return new UserAuthResponseModel
            {
                AccessToken = newAccessToken,
                RefreshToken = newRefreshToken
            };
        }

    }
}
