using AutoMapper;
using BusinessObjects.DTOs.User.Request;
using BusinessObjects.DTOs.User.Response;
using BusinessObjects.DTOs.UserDTOs.Request;
using BusinessObjects.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Mappers
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            //User
            CreateMap<UserRegisterWithPwRequestModel, User>();
            CreateMap<User, UserOwnInfoResponseModel>();
            CreateMap<UserUpdateRequestModel, User>();
        }
    }
}
