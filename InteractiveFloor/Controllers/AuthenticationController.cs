using BusinessObjects.DTOs.User.Request;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Service.Services.AuthenticationServices;

namespace InteractiveFloor.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly IAuthenticationService _authenticationService;

        public AuthenticationController(IAuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
        }

        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register(UserRegisterWithPwRequestModel request)
        {
            await _authenticationService.RegisterWithPassword(request);
            return Ok("Đăng ký tài khoản thành công.");
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login(UserLoginRequestModel request)
        {
            var response = await _authenticationService.LoginAuthenticate(request);
            return Ok(response);
        }

        [HttpPut]
        [Route("refresh-token")]
        public async Task<IActionResult> ValidateRefreshToken([FromBody] string refreshToken)
        {
            var response = await _authenticationService.CheckRefreshToken(refreshToken);
            return Ok(response);
        }
    }
}
