using AutoMapper;
using BusinessObjects.DTOs.User.Response;
using Repository.Repositories.UserRepositories;
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
            return _mapper.Map<UserOwnInfoResponseModel>(user);
        }
    }
}
