using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShrinkLinkCore.DataTransferObjects
{
    public class UserDto
    {
        public Guid Id { get; set; }
        public string? CodeWord { get; set; }
        public string? Email { get; set; }
        public string? PasswordHash { get; set; }
        public string? Name { get; set; }
        public string? Surname { get; set; }
        public string? PhoneNumber { get; set; }

        [DataType(DataType.Date)]
        public DateTime? BirthDate { get; set; }
        public string? Address { get; set; }
        public DateTime RegistrationDate { get; set; }
        public DateTime LastLogin { get; set; }
        public bool? IsFullBlocked { get; set; }
        public bool? IsOnline { get; set; }
        public List<LinkDto>? Links { get; set; }
        public List<RoleDto>? Roles { get; set; }
    }
}
