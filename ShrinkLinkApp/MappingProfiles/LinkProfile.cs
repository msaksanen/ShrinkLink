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

        }

    }
}
