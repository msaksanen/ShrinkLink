using System.ComponentModel.DataAnnotations;

namespace ShrinkLinkApp.Models
{
    public class ChangePasswordModel :UserModel
    {
        [Required]
        [DataType(DataType.Password)]
        public string? OldPassword { get; set; }
        public string? SystemInfo { get; set; }

        public string? OldPwdInfo { get; set; }
    }
}
