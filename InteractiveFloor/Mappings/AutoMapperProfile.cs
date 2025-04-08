using AutoMapper;
using BusinessObjects.Models;
using DTO;

namespace InteractiveFloor.Mappings
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            // Game Category 
            CreateMap<GameCategory, GameCategoryResponse>();
            CreateMap<GameCategory, GameCategoryInGameResponse>();
            CreateMap<CreateGameCategoryRequest, GameCategory>();
            CreateMap<UpdateGameCategoryRequest, GameCategory>();

            // Game Version 
            CreateMap<GameVersion, GameVersionResponse>();
            CreateMap<CreateGameVersionRequest, GameVersion>();
            CreateMap<AddGameVersionRequest, GameVersion>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => Guid.NewGuid().ToString()));

            // Game 
            CreateMap<Game, GameResponse>()
                .ForMember(dest => dest.Categories, opt => opt.MapFrom(src => 
                    src.GameCategoryRelations.Select(gcr => gcr.GameCategory)))
                .ForMember(dest => dest.Versions, opt => opt.MapFrom(src => src.GameVersions));

            CreateMap<CreateGameRequest, Game>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => Guid.NewGuid().ToString()))
                .ForMember(dest => dest.PlayCount, opt => opt.MapFrom(src => 0))
                .ForMember(dest => dest.GameCategoryRelations, opt => opt.Ignore())
                .ForMember(dest => dest.GameVersions, opt => opt.Ignore());

            CreateMap<UpdateGameRequest, Game>()
                .ForMember(dest => dest.GameCategoryRelations, opt => opt.Ignore())
                .ForMember(dest => dest.GameVersions, opt => opt.Ignore());
        }
    }
} 