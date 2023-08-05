using ShrinkLinkCore.DataTransferObjects;

namespace ShrinkLinkApp.Models
{
    public class RedirectLinkModel
    {
        public string? ShortLink { get; set; }
        public DateTime ExpirationDate { get; set; }
        public string? LocalURL { get; set; }
        public int SystemFlag { get; set; }
        public string? SystemInfo { get; set; }
    }
}
