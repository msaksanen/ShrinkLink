using MediatR;
using ShrinkLinkCore.DataTransferObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShrinkLinkCQS.Links.Queries
{
    public class GetLinkByShortIdQuery : IRequest<LinkDto?>
    {
        public string? ShortId { get; set; }
    }
}

