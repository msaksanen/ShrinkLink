using MediatR;
using ShrinkLinkCore.DataTransferObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShrinkLinkCQS.Links.Commands
{
    public class SaveLinkCommand : IRequest<int?>
    {
        public LinkDto? Dto { get; set; }
    }
}
