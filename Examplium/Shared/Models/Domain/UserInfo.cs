using System.ComponentModel.DataAnnotations;

namespace Examplium.Shared.Models.Domain
{
    public class UserInfo
    {
        [Key]
        public int Id { get; set; }
        public string UserId { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public DateTime Created { get; set; } = DateTime.UtcNow;
        public DateTime Updated { get; set; } = DateTime.UtcNow;

        public string FirstName { get; set; } = string.Empty;
        public string MiddleName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;

        public string Picture { get; set; } = string.Empty;

        public string TimeZone { get; set; } = string.Empty;
        public int Language { get; set; }

    }
}
