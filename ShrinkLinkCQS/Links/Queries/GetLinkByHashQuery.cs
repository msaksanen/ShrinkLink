using MediatR;
using ShrinkLinkCore.DataTransferObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShrinkLinkCQS.Links.Queries
{
    public class GetLinkByHashQuery : IRequest<LinkDto?>
    {
        public string? Hash { get; set; }
    }
}
