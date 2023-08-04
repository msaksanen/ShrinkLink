using ShrinkLinkCore.DataTransferObjects;

namespace ShrinkLinkApp.Models
{
    public class LinkModel
    {
        public Guid Id { get; set; }
        public string? URL { get; set; }
        public string? Hash { get; set; }
        public string? ShortId { get; set; }
        public int? Counter { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime ExpirationDate { get; set; }
        public List<UserDto>? Users { get; set; }
    }
}
