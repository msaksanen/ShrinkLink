using MediatR;
using ShrinkLinkCore.DataTransferObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShrinkLinkCQS.Links.Commands
{
    public class PatchLinkCommand: IRequest<int?>
    {
        public Guid Id { get; set; }
        public Dictionary<string, object?> nameValuePropertiesPairs = new Dictionary<string, object?>();
    }   
}
