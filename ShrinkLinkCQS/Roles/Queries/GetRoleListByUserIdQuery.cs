using MediatR;
using ShrinkLinkCore.DataTransferObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShrinkLinkCQS.Roles.Queries
{
    public class GetRoleListByUserIdQuery : IRequest<IEnumerable<RoleDto>?>
    {
        public Guid? UserId { get; set; }
    }
}
