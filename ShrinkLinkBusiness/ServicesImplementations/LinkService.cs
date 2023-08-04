using AutoMapper;
using MediatR;
using Microsoft.Extensions.Configuration;
using shortid;
using shortid.Configuration;
using ShrinkLinkCore.DataTransferObjects;
using ShrinkLinkCQS.Links.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ShrinkLinkBusiness.ServicesImplementations
{
    public class LinkService
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

        public async Task<LinkDto> GenerateShorLinkAsync(LinkDto newLink)
        {
            var oldLink = await _mediator.Send(new GetLinkByHashQuery() { Hash = newLink.URL });
            if (oldLink != null && string.Compare(newLink.URL, oldLink.URL,StringComparison.InvariantCulture)==0)
                return oldLink;

            var shortId = await GenerateShortIdAsync();
            newLink.ShortId = shortId;

        }

        private async Task <string> GenerateShortIdAsync()
        { 
            int lengthId = 8;
            string shortId = string.Empty;  
            var optsLength = _configuration["ShrinkLink:Length"];
            if (int.TryParse (optsLength, out int optL) && optL>5)
                lengthId = optL;
            var options = new GenerationOptions(useSpecialCharacters: false, length: lengthId);
            var charSet = _configuration["ShrinkLink:CharSet"];
            if (charSet != null && !string.IsNullOrEmpty(charSet))
                ShortId.SetCharacters(charSet);
            do
            {
                shortId = ShortId.Generate(options);
            }
            while (await _mediator.Send(new GetLinkByShortIdQuery() { ShortId = shortId })!= null);

          return shortId;
        }

      

    }
}
