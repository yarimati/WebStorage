namespace WebStorage.Models
{
    public class UserLink
    {
        public int UserLinkId { get; set; }
        public string Link { get; set; }
        public AppUser AppUser { get; set; }
    }
}
