# Add UserInfo Entity

### Add model class

If it doesnt exist, in the Examplium.Shared/Models folder create a new folder and rename it to "Domain".

Add a new class with the file name UserInfo.cs.

Update the content with this code:
```
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
```

<br/>

### Add DbSet

In the Examplium.Server/Data/ApplicationDbContext.cs file add a DbSet with the UserInfo type, like this:
```
using Examplium.Shared.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace Examplium.Server.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Note> Notes { get; set; }
        public DbSet<UserInfo> UserInfos { get; set; }
    }
}
```

<br/>

### Add migration and update database

In the package manager console enter these two commands:
```
add-migration AddUserInfosMigration -Project Examplium.Server -Context ApplicationDbContext
```

```
update-database -Project Examplium.Server -Context ApplicationDbContext
```
<br/>
