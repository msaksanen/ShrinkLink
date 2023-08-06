using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShrinkLinkDb.Entities
{
    public class User : IBaseEntity
    {
        public Guid Id { get; set; }
        public string? CodeWord { get; set; }
        public string? Email { get; set; }
        public string? PasswordHash { get; set; }
        public string? Name { get; set; }
        public string? Surname { get; set; }
        public string? PhoneNumber { get; set; }
        public DateTime? BirthDate { get; set; }
        public string? Address { get; set; }
        public DateTime RegistrationDate { get; set; }
        public DateTime LastLogin { get; set; }
        public bool? IsFullBlocked { get; set; }
        public bool? IsOnline { get; set; }
        public List<Link>? Links { get; set; }
        public List<Role>? Roles { get; set; }

    }
}
