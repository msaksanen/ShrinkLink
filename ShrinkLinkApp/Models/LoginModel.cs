using System.ComponentModel.DataAnnotations;

namespace ShrinkLinkApp.Models
{
    public class LoginModel
    {
        [Required]
        [EmailAddress]
        public string? Email { get; set; }

        [Required(ErrorMessage = "The field is required. Input something anyway")]
        [DataType(DataType.Password)]
        public string? Password { get; set; }
        public string? SystemInfo { get; set; }

    }
}

