using System.Linq;
using AatingApp.API.Dtos;
using AatingApp.API.Models;
using AutoMapper;

namespace AatingApp.API.Helpers
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<User, UserForListDto>()
                    .ForMember(dest => dest.PhotoUrl, opt => {
                        opt.MapFrom( src => src.Photos.FirstOrDefault( predicate =>predicate.IsMain ).Url);
                    })
                    .ForMember( dest => dest.Age,
                                   opt => opt.ResolveUsing(d => d.DateOfBirth.CalculateAge()) );
                    
            CreateMap<User, UserForDetailedDto>()
                    .ForMember(dest => dest.PhotoUrl, opt => {
                        opt.MapFrom( src => src.Photos.FirstOrDefault( predicate =>predicate.IsMain ).Url);
                    })
                    
                    .ForMember( dest => dest.Age,
                                   opt => opt.ResolveUsing(d => d.DateOfBirth.CalculateAge()) );
            CreateMap<Photo, PhotosForDetailedDto>();
        }
    }
}