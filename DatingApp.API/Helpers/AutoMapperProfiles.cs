using System.Linq;
using AutoMapper;
using DatingApp.API.Dtos;
using DatingApp.API.Models;

namespace DatingApp.API.Helpers
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
           // you config source object on the left & destination object on the right 
           CreateMap<User, UserForListDto>()
                    .ForMember(dest => dest.PhotoUrl,
                               opt => opt.MapFrom(src => src.Photos.FirstOrDefault(p => p.IsMain).Url))
                    .ForMember(dest => dest.Age, 
                               opt => { opt.MapFrom(d => d.DateOfBirth.CalculateAge());
                               });            
           CreateMap<User, UserForDetailedDto>()
                    .ForMember(dest => dest.PhotoUrl,
                               opt => opt.MapFrom(src => src.Photos.FirstOrDefault(p => p.IsMain).Url))
                    .ForMember(dest => dest.Age, 
                                opt => { opt.MapFrom(d => d.DateOfBirth.CalculateAge());
                                });            
           CreateMap<Photo, PhotosForDetailedDto>();
           CreateMap<UserForUpdateDto, User>();
           //new mapping with photos as the source & photoforreturndto as the mapping destination
           CreateMap<Photo, PhotoForReturnDto>();
           CreateMap<PhotoForCreationDto, Photo>();
        }
    }
}