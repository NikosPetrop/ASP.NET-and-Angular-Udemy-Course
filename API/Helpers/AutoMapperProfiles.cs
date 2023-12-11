using API.DTOs;
using API.Entities;
using API.Extension;
using AutoMapper;

namespace API.Helpers;

public class AutoMapperProfiles : Profile
{
    /// <summary> We specify class we want to map </summary>
    public AutoMapperProfiles()
    {
        CreateMap<AppUser, MemberDTO>()
            //We explicitly map an uknown (to automapper) member to get what we want, in this case just the url of main photo
            .ForMember(dest => dest.PhotoUrl, 
                opt => opt.MapFrom(src => src.Photos.FirstOrDefault(x => x.IsMain).Url))
            // Here we optimize GetAge to not include dependant members we don't need
            .ForMember(dest => dest.Age,
                opt=> opt.MapFrom(src=>src.DateOfBirth.CalculateAge()));
        CreateMap<Photo, PhotoDTO>();
    }
}
