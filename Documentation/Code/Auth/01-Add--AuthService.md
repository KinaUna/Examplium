# AuthService

<br/>

### Add Auth folder

If it doesn't exist, create a new folder in Examplium.Server root folder and name it "Services".
Add a new folder here, with the name "Auth".

<br/>

### Create IAuthService interface
In the Auth folder add a new interface and name it IAuthService.cs
Code:
```
namespace Examplium.Server.Services.Auth
{
    public interface IAuthService
    {
        string? GetUserId();
        string? GetUserEmail();
    }
}
```

<br/>

### Create AuthService class
In the Auth folder add a new class and name it AuthService.cs
Code:
```
using Duende.IdentityServer.Extensions;

namespace Examplium.Server.Services.Auth
{
    public class AuthService: IAuthService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AuthService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public string? GetUserId() => _httpContextAccessor.HttpContext?.User.Identity.GetSubjectId();

        public string? GetUserEmail() => _httpContextAccessor.HttpContext?.User.FindFirst("email")?.Value;
    }
}
```

<br/>

### Register services

Open Examplium.Server/Program.cs, go to the line just above `var app = builder.Build();` 

Add the HttpContextAccessor service:
```
builder.Services.AddHttpContextAccessor();
```

Register the AuthService:
```
builder.Services.AddScoped<IAuthService, AuthService>();
```

