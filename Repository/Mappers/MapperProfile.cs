using AutoMapper;
using BusinessObjects.DTOs.Device.Request;
using BusinessObjects.DTOs.DeviceCategory.Request;
using BusinessObjects.DTOs.DeviceCategory.Response;
using BusinessObjects.DTOs.Game;
using BusinessObjects.DTOs.GameCategory;
using BusinessObjects.DTOs.GamePackage.Request;
using BusinessObjects.DTOs.GamePackage.Response;
using BusinessObjects.DTOs.GamePackageOrder.Request;
using BusinessObjects.DTOs.GamePackageOrder.Response;
using BusinessObjects.DTOs.InteractiveFloor.Request;
using BusinessObjects.DTOs.InteractiveFloor.Response;
using BusinessObjects.DTOs.Organization.Request;
using BusinessObjects.DTOs.Organization.Response;
using BusinessObjects.DTOs.User.Request;
using BusinessObjects.DTOs.User.Response;
using BusinessObjects.DTOs.UserPackage.Request;
using BusinessObjects.DTOs.UserPackage.Response;
using BusinessObjects.DTOs.UserPackageOrder.Request;
using BusinessObjects.DTOs.UserPackageOrder.Response;
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
            CreateMap<InfoUpdateRequestModel, User>();
            CreateMap<StaffCreateRequestModel, User>();
            CreateMap<User, UserInfoResponeModel>();
            CreateMap<User, OrganizationUserReponseModel>();

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


            //Organization
            CreateMap<OrganizationCreateUpdateRequestModel, Organization>();
            CreateMap<Organization, OrganizationInfoResponseModel>();

            //Game Package
            CreateMap<GamePackageCreateRequestModel, GamePackage>();
            CreateMap<GamePackageUpdateRequestModel, GamePackage>();
            CreateMap<GamePackage, GamePackageListResponseModel>();
            CreateMap<Game, GameInfo>()
                .ForMember(dest => dest.Categories, opt => opt.MapFrom(src =>
                    src.GameCategoryRelations.Select(gcr => gcr.GameCategory)))
                .ForMember(dest => dest.Versions, opt => opt.MapFrom(src => src.GameVersions));

            //User Package
            CreateMap<UserPackageCreateUpdateRequestModel, UserPackage>();
            CreateMap<UserPackage, UserPackageListResponseModel>();

            //Floor
            CreateMap<FloorCreateUpdateRequestModel, InteractiveFloor>();
            CreateMap<InteractiveFloor, FloorDetailsInfoResponseModel>();

            //DeviceCategory
            CreateMap<DeviceCategoryCreateUpdateRequestModel, DeviceCategory>();
            CreateMap<DeviceCategory, DeviceCategoryInfoResponseModel>();

            //Device
            CreateMap<DeviceCreateUpdateRequestModel, Device>();
            CreateMap<Device, DeviceInfo>();

            //Game Package Order
            CreateMap<GamePackageOrderCreateRequestModel, GamePackageOrder>();
            CreateMap<GamePackageOrder, GamePackageOrderListResponseModel>();

            //UserPackageOrder
            CreateMap<UserPackageOrderCreateRequestModel, UserPackageOrder>();
            CreateMap<UserPackageOrder, UserPackageOrderListResponseModel>();
        }
    }
}
