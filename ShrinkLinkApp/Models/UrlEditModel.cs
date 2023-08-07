using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace ShrinkLinkApp.Models
{
    public class UrlEditModel
    {
        public Guid LinkId { get; set; }
        public Guid UserId { get; set; }
        public string? URL { get; set; }
        public string? LocalURL { get; set; }
        public string? Hash { get; set; }
        public string? ShortId { get; set; }
        public int? Counter { get; set; }
        public DateTime CreationDate { get; set; }

        [DataType(DataType.DateTime)]
        [Remote(action: "CheckExpDate", controller: "UserCheck", ErrorMessage = "Input correct expiration time for a short URL")]
        public DateTime ExpDateTime { get; set; }
    }
}
