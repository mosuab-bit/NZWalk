using AutoMapper;
using NZ_Walk.Models.Domain;
using NZ_Walk.Models.DTO;

namespace NZ_Walk.Mappings
{
    public class AutoMapperProfile:Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Region,RegionDTO>().ReverseMap();
            CreateMap<AddRegionRequestDto,Region>().ReverseMap();
            CreateMap<UpdateRegionRequestDto, Region>().ReverseMap();
            CreateMap<AddWalksRequestDto,Walk>().ReverseMap();
            CreateMap<WalkDto,Walk>().ReverseMap();
            CreateMap<Difficulty,DifficultyDto>().ReverseMap();
            CreateMap<UpdateWalkDto,Walk>().ReverseMap();
        }
    }
}
