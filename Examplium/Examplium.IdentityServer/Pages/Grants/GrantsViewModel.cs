namespace Examplium.IdentityServer.Pages.Grants
{
    public class GrantsViewModel
    {
        public IEnumerable<GrantViewModel>? Grants { get; set; }
    }

    public class GrantViewModel
    {
        public string ClientId { get; set; } = string.Empty;
        public string ClientName { get; set; } = string.Empty;
        public string ClientUrl { get; set; } = string.Empty;
        public string ClientLogoUrl { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTime Created { get; set; }
        public DateTime? Expires { get; set; }
        public IEnumerable<string>? IdentityGrantNames { get; set; }
        public IEnumerable<string>? ApiGrantNames { get; set; }
    }
}
