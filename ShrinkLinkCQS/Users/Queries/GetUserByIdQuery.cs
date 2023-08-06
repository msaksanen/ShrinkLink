using MediatR;
using ShrinkLinkCore.DataTransferObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShrinkLinkCQS.Users.Queries
{
    public class GetUserByIdQuery : IRequest<UserDto?>
    {
        public Guid? UserId { get; set; }
    }
}


