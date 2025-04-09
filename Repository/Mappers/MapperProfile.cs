using AutoMapper;
using BusinessObjects.DTOs.User.Request;
using BusinessObjects.DTOs.User.Response;
using BusinessObjects.DTOs.UserDTOs.Request;
using BusinessObjects.Models;
using DTO;
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
            CreateMap<InfoUpdateRequestModel, User>();
            CreateMap<StaffCreateRequestModel, User>();

            //Game Category 
            CreateMap<GameCategory, GameCategoryResponse>();
            CreateMap<GameCategory, GameCategoryInGameResponse>();
            CreateMap<CreateGameCategoryRequest, GameCategory>();
            CreateMap<UpdateGameCategoryRequest, GameCategory>();

            //Game Version 
            CreateMap<GameVersion, GameVersionResponse>();
            CreateMap<CreateGameVersionRequest, GameVersion>();

            //Game 
            CreateMap<Game, GameResponse>()
                .ForMember(dest => dest.Categories, opt => opt.MapFrom(src =>
                    src.GameCategoryRelations.Select(gcr => gcr.GameCategory)))
                .ForMember(dest => dest.Versions, opt => opt.MapFrom(src => src.GameVersions));

            CreateMap<CreateGameRequest, Game>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => Guid.NewGuid().ToString("N").Substring(0, 32)))
                .ForMember(dest => dest.PlayCount, opt => opt.MapFrom(src => 0))
                .ForMember(dest => dest.GameCategoryRelations, opt => opt.Ignore())
                .ForMember(dest => dest.GameVersions, opt => opt.Ignore());

            CreateMap<UpdateGameRequest, Game>()
                .ForMember(dest => dest.GameCategoryRelations, opt => opt.Ignore())
                .ForMember(dest => dest.GameVersions, opt => opt.Ignore());
        }
    }
}
