# Add Notes Entity

### Add model class

In the Examplium.Shared/Models folder create a new folder and rename it to "Domain".

Add a new class with the file name Note.cs.

Update the content with this code:
```
namespace Examplium.Shared.Models.Domain
{
    public class Note
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
        public string Author { get; set; } = string.Empty;
        public string Tags { get; set; } = string.Empty;
        public DateTime Created { get; set; } = DateTime.UtcNow;
        public DateTime Updated { get; set; } = DateTime.UtcNow;
    }
}
```

<br/>

### Add DbSet

In the Examplium.Server/Data/ApplicationDbContext.cs file add a DbSet with the Note type, like this:
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
    }
}
```

<br/>

### Add migration and update database

In the package manager console enter these two commands:
```
add-migration AddNotesMigration -Project Examplium.Server -Context ApplicationDbContext
```

```
update-database -Project Examplium.Server -Context ApplicationDbContext
```
<br/>

