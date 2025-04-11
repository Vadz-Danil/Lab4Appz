using AutoMapper;
using SightSeeing.Entities.DTO;
using SightSeeing.Entities;

namespace SightSeeing.BLL.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<User, UserDto>().ReverseMap();
            CreateMap<Place, PlaceDto>().ReverseMap();
            CreateMap<Review, ReviewDto>().ReverseMap();
        }
    }
}