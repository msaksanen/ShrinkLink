namespace ShrinkLinkApp.Models
{
    public class ShortenModel
    {
        public Guid Id { get; set; }
        public string? OriginalLink { get; set; }
        public string? ShortlLink { get; set; }
        public DateTime ExpirationDate { get; set; }
        public string? LocalURL { get; set; }
        public int SystemFlag { get; set; }
        public string? SystemInfo { get; set; }
    }
}
