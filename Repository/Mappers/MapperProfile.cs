using AutoMapper;
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
            CreateMap<UserRegisterWithPwRequestModel, User>();
        }
    }
}
