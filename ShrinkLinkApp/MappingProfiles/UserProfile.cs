using AutoMapper;
using ShrinkLinkApp.Models;
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

            CreateMap<UserModel, UserDto>()
                  .ForMember(dto => dto.RegistrationDate,
                    opt => opt.MapFrom(model => model.RegistrationDate ?? DateTime.Now))
                  .ForMember(dto => dto.Id,
                    opt => opt.MapFrom(model => model.Id ?? Guid.NewGuid()))
                  .ForMember(dto => dto.PasswordHash,
                    opt => opt.MapFrom(model => model.Password));


        }

    }
}
