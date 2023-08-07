using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShrinkLinkCQS.Links.Commands
{
    public class AddLinkIdToUserByIdCommand : IRequest<int>
    {
        public Guid? UserId { get; set; }
        public Guid? LinkId { get; set; }
    }
}
