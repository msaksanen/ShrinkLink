namespace ShrinkLinkApp.Models
{
    public class UserDataModel
    {
        public Guid Id { get; set; }
        public string? FullName { get; set; }
        public string? Email { get; set; }
        public List<string>? RoleNames { get; set; }
    }
}
