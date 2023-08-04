using MediatR;
using ShrinkLinkCore.DataTransferObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShrinkLinkCQS.Links.Commands
{
    public class UpdateLinkCommand : IRequest<int?>
    {
        public LinkDto? dto { get; set; }
    }
}
