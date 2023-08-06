using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace ShrinkLinkApp.Models
{
    public class UserModel
    {
        public Guid? Id { get; set; }

        [Required]
        [MinLength(3)]
        [MaxLength(32)]
        public string? CodeWord { get; set; }

        [Required]
        [RegularExpression(@"[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,4}")]
        [Remote(action: "CheckEmail", controller: "UserCheck", ErrorMessage = "Email is already in use")]
        public virtual string? Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string? Password { get; set; }

        [Compare(nameof(Password))]
        [DataType(DataType.Password)]
        public string? PasswordConfirmation { get; set; }

        [Required]
        [Phone]
        public string? PhoneNumber { get; set; }

        [MaxLength(32)]
        public string? Name { get; set; }

        [MaxLength(32)]
        public string? Surname { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [Remote(action: "CheckBirthDate", controller: "UserCheck", ErrorMessage = "Input correct date of birth")]
        public DateTime? BirthDate { get; set; }

        [Required]
        [MinLength(3)]
        [MaxLength(256)]
        public string? Address { get; set; }
        public bool? IsFullBlocked { get; set; }
        public bool? IsOnline { get; set; }
        public Guid? RoleId { get; set; }
        public List<string>? RoleNames { get; set; }
        // public virtual string? RoleName { get; set; }
        public DateTime? RegistrationDate { get; set; }
        public DateTime LastLogin { get; set; }

    }
}
