using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShrinkLinkCore.DataTransferObjects
{
    public class UserDto
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public string? Surname { get; set; }
        public string? Email { get; set; }
        public string? PasswordHash { get; set; }
        public DateTime RegistrationDate { get; set; }
        public DateTime LastLogin { get; set; }
        public List<LinkDto>? Links { get; set; }
    }
}
