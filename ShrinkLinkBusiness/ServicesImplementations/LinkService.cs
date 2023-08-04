using AutoMapper;
using Azure.Core;
using MediatR;
using Microsoft.CodeAnalysis;
using Microsoft.Extensions.Configuration;
using shortid;
using shortid.Configuration;
using ShrinkLinkCore.Abstractions;
using ShrinkLinkCore.DataTransferObjects;
using ShrinkLinkCQS.Links.Commands;
using ShrinkLinkCQS.Links.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ShrinkLinkBusiness.ServicesImplementations
{
    public class LinkService : ILinkService
    {
        private readonly IMapper _mapper;
        private readonly IMediator _mediator;
        private readonly IConfiguration _configuration;

        public LinkService(IConfiguration configuration, IMapper mapper, IMediator mediator)
        {
            _mapper = mapper;
            _configuration = configuration;
            _mediator = mediator;
        }

        public async Task<LinkGenObjExtend> GenerateShorLinkAsync(LinkDto dto)
        {
            LinkGenObjExtend linkObj = new();

            var oldLink = await _mediator.Send(new GetLinkByHashQuery() { Hash = dto.Hash });
            if (oldLink != null && string.Compare(dto.URL, oldLink.URL,StringComparison.InvariantCulture)==0)
            {
                if (oldLink.ExpirationDate < DateTime.Now)
                {
                   //var newTime = dto.ExpirationDate;
                   //var res = await _mediator.Send(new ChangeLinkExpTimeCommand() { ExpTime = newTime, Id = oldLink.Id });

                    oldLink.ExpirationDate = dto.ExpirationDate;
                    //var res = await _mediator.Send(new UpdateLinkCommand() { dto = oldLink });
                    var res = await _mediator.Send(new PatchLinkCommand() 
                    { Id = oldLink.Id, nameValuePropertiesPairs = new Dictionary<string, object?>() { ["ExpirationDate"] = dto.ExpirationDate} });
                    if (res > 0)
                    {
                        //oldLink.ExpirationDate = newTime;
                        linkObj.ExpDateUpdResult = res;
                        linkObj.Link = oldLink;
                    }
                    else
                    {
                        linkObj = await GenerateLinkObjAsync(dto);
                    }
                    return linkObj;
                }
                linkObj.Link = oldLink;
                return linkObj;
            }

            return await GenerateLinkObjAsync(dto);
        }

        private async Task<LinkGenObjExtend> GenerateLinkObjAsync(LinkDto dto)
        {
            int lengthId = 8;
            string shortId = string.Empty;
            LinkDto? tempEntity = null;
            Result? newRes = new();
            var sb = new StringBuilder();
            var optsLength = _configuration["ShrinkLink:Length"];
            if (int.TryParse(optsLength, out int optL) && optL > 5)
                lengthId = optL;
            var options = new GenerationOptions(useSpecialCharacters: false, length: lengthId);
            newRes.Length = lengthId;
            var charSet = _configuration["ShrinkLink:CharSet"];
            if (charSet != null && !string.IsNullOrEmpty(charSet))
                ShortId.SetCharacters(charSet);

            for (int i = 0; i >= 0; i++)
            {
                shortId = ShortId.Generate(options);
                tempEntity = await _mediator.Send(new GetLinkByShortIdQuery() { ShortId = shortId });
                if (tempEntity != null)
                {
                    sb.Append($"Id: {tempEntity.Id}; ShortId: {tempEntity.ShortId})\n");
                    newRes.CountResult++;
                }
                else break;
            }
            newRes.Text = sb.ToString();    
            dto.ShortId = shortId;
            var res = await _mediator.Send(new SaveLinkCommand() { Dto = dto });
            newRes.SaveResult = res;

            LinkGenObjExtend newLink = new()
            {
                Link = dto,
                GenResult = newRes
            };

            return newLink;
        }


    }
}
