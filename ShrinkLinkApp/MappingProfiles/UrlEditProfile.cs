using AutoMapper;
using ShrinkLinkApp.Models;
using ShrinkLinkCore.DataTransferObjects;

namespace ShrinkLinkApp.MappingProfiles
{
    public class UrlEditProfile : Profile
    {
        public UrlEditProfile()
        {
            CreateMap<(UserDto, LinkDto), UrlEditModel>()
             .ForMember(m => m.UserId,
              opt => opt.MapFrom(s => s.Item1.Id))
             .ForMember(m => m.LinkId,
              opt => opt.MapFrom(s => s.Item2.Id))
             .ForMember(m => m.ShortId,
              opt => opt.MapFrom(s => s.Item2.ShortId))
             .ForMember(m => m.URL,
              opt => opt.MapFrom(s => s.Item2.URL))
             .ForMember(m => m.Counter,
              opt => opt.MapFrom(s => s.Item2.Counter))
             .ForMember(m => m.CreationDate,
              opt => opt.MapFrom(s => s.Item2.CreationDate))
             .ForMember(m => m.ExpDateTime,
              opt => opt.MapFrom(s => s.Item2.ExpirationDate));


        }

    }
}
