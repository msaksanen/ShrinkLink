using AutoMapper;
using ShrinkLinkCore.DataTransferObjects;
using ShrinkLinkDb.Entities;

namespace ShrinkLinkApp.MappingProfiles
{
   
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<User, UserDto>();

            CreateMap<UserDto, User>().ReverseMap();



        }

    }
}
