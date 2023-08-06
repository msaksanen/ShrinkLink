using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace ShrinkLinkApp.Models
{
    public class LinkInputModel
    {
        public string? URL { get; set; }

        [DataType(DataType.DateTime)]
        [Remote(action: "CheckExpDate", controller: "UserCheck", ErrorMessage = "Input correct expiration time for a short URL")]
        public DateTime? ExpDateTime { get; set; }
    }
}
