using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShrinkLinkCQS.Links.Commands
{
    public class ChangeLinkExpTimeCommand : IRequest<int?>
    {
        public Guid? Id { get; set; }
        public DateTime ExpTime { get; set; }
    }
}
