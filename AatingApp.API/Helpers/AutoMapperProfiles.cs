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

            CreateMap<UserForUpdateDto, User>();

            CreateMap<Photo, PhotoForReturnDto>();

            CreateMap<PhotoFroCreationDto, Photo>();
            
            CreateMap<UserForRegisterDto, User>();

            CreateMap<MessageForCreateionDtoo, Message>().ReverseMap();

            CreateMap<Message, MessageToReturnDto>()
                                .ForMember(m => m.SenderPhotoUrl,
                                 opt => opt.MapFrom( u=> u.Sender.Photos.FirstOrDefault(p =>p.IsMain).Url))
                                 .ForMember( m=> m.RecipientPhotoUrl, opt => opt
                                .MapFrom( u=> u.Recipient.Photos.FirstOrDefault(p => p.IsMain).Url ));
            
        }
    }
}