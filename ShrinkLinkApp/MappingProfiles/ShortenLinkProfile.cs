using AutoMapper;
using ShrinkLinkApp.Models;
using ShrinkLinkCore.DataTransferObjects;
using ShrinkLinkDb.Entities;

namespace ShrinkLinkApp.MappingProfiles
{
    public class ShortenLinkProfile : Profile
    {
        public ShortenLinkProfile()
        {
            CreateMap<LinkGenObjExtend, ShortenModel>()
                .ForMember(i => i.ExpirationDate, d => d.MapFrom(dt => (dt.Link != null) ? dt.Link.ExpirationDate : DateTime.MinValue))
                .ForMember(i => i.OriginalLink, d => d.MapFrom(dt => (dt.Link != null) ? dt.Link.URL : string.Empty))
                .ForMember(i => i.ShortlLink, d => d.MapFrom(dt => (dt.Link != null) ? dt.Link.ShortId : string.Empty))
                .ForMember(i => i.Id, d => d.MapFrom(dt => (dt.Link != null) ? dt.Link.Id : Guid.Empty))
                .ForMember(i => i.SystemFlag, d => d.MapFrom(dt => (dt.ExpDateUpdResult != null) ? 100 : (dt.GenResult != null ? dt.GenResult.SaveResult : 1000)));
    }

    }
}
