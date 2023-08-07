using MediatR;
using ShrinkLinkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShrinkLinkCQS.Users.Commands
{
    public class PatchUserDataCommand : IRequest<int?>
    {
        public Guid? UserId { get; set; }
        public List<PatchModel>? patchData { get; set; }
    }
}
