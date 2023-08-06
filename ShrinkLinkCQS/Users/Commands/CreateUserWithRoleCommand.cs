using MediatR;
using ShrinkLinkCore.DataTransferObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShrinkLinkCQS.Users.Commands
{
    public class CreateUserWithRoleCommand : IRequest<int?>
    {
        public UserDto? UserDto { get; set; }
        public string? RoleName { get; set; }
    }
}
