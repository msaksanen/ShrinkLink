using AutoMapper;
using ShrinkLinkApp.Models;
using ShrinkLinkCore.DataTransferObjects;
using ShrinkLinkDb.Entities;

namespace ShrinkLinkApp.MappingProfiles
{
    public class LinkProfile : Profile
    {
        public LinkProfile()
        {
            CreateMap<Link, LinkDto>();

            CreateMap<LinkDto, Link>().ReverseMap();

            CreateMap<LinkModel, LinkDto>();

            CreateMap<LinkDto, LinkModel>().ReverseMap();

            CreateMap<LinkDto, RedirectLinkModel>()
                .ForMember(i => i.ShortLink, d => d.MapFrom(dt => dt.ShortId))
                .ForMember(i => i.ExpirationDate, d => d.MapFrom(dt => dt.ExpirationDate));


        }

    }
}
