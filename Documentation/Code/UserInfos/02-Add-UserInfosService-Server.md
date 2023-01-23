# Add UserInfos Service on Examplium.Server

This service depends on the ServiceResponse class, if it hasn't been created yet, see: https://github.com/KinaUna/Examplium/blob/master/Documentation/Code/Common/01-ServiceResponse.md

This service depends on the AuthService, if it hasn't been created yet, see: https://github.com/KinaUna/Examplium/blob/master/Documentation/Code/Auth/01-Add--AuthService.md

<br/>

### Add folders

In the Examplium.Server root add a new folder named "Services", if it doesn't already exists.

In the Services folder add a folder with the name "UserInfos".

<br/>

### Add IUserInfosService.cs interface

In the UserInfos folder add a new interface file with the name "IUserInfosService.cs" and replace the content with this:
```
using Examplium.Shared.Models.Domain;
using Examplium.Shared.Models.Services;

namespace Examplium.Server.Services.UserInfos
{
    public interface IUserInfosService
    {
        Task<ServiceResponse<UserInfo>> GetCurrentUserInfo();
        Task<ServiceResponse<UserInfo>> UpdateCurrentUserInfo(UserInfo userInfo);
    }
}
```

<br/>

### Add UserInfosService.cs

In the UserInfos folder add a new class file with the name "UserInfoService.cs" and replace the content with this:

```
using Examplium.Server.Data;
using Examplium.Server.Services.Auth;
using Examplium.Shared.Models.Domain;
using Examplium.Shared.Models.Services;
using Microsoft.EntityFrameworkCore;

namespace Examplium.Server.Services.UserInfos
{
    public class UserInfosService: IUserInfosService
    {
        private readonly ApplicationDbContext _context;
        private readonly IAuthService _authService;

        public UserInfosService(ApplicationDbContext context, IAuthService authService)
        {
            _context = context;
            _authService = authService;
        }

        public async Task<ServiceResponse<UserInfo>> GetCurrentUserInfo()
        {
            ServiceResponse<UserInfo> response = new ServiceResponse<UserInfo>();

            string? currentUserId = _authService.GetUserId();

            if (!string.IsNullOrEmpty(currentUserId))
            {
                UserInfo? userInfo = await _context.UserInfos.SingleOrDefaultAsync(u => u.UserId == currentUserId);

                if (userInfo == null)
                {
                    userInfo = new UserInfo();
                    userInfo.UserId = currentUserId;
                    userInfo.Email = _authService.GetUserEmail()!;
                    userInfo.Updated = DateTime.UtcNow;
                    userInfo.Created = DateTime.UtcNow;

                    await _context.UserInfos.AddAsync(userInfo);
                    await _context.SaveChangesAsync();
                }

                response.Data = userInfo;
                return response;
            }

            response.Success = false;
            response.Message = "Error: Invalid user data";
            return response;
        }

        public async Task<ServiceResponse<UserInfo>> UpdateCurrentUserInfo(UserInfo userInfo)
        {
            ServiceResponse<UserInfo> response = new ServiceResponse<UserInfo>();

            string? currentUserId = _authService.GetUserId();
            
            if (!string.IsNullOrEmpty(currentUserId) && userInfo.UserId == currentUserId)
            {
                UserInfo? currentUserInfo = await _context.UserInfos.SingleOrDefaultAsync(u => u.UserId == currentUserId);
                if (currentUserInfo != null)
                {
                    currentUserInfo.FirstName = userInfo.FirstName;
                    currentUserInfo.MiddleName = userInfo.MiddleName;
                    currentUserInfo.LastName = userInfo.LastName;
                    currentUserInfo.Picture = userInfo.Picture;
                    currentUserInfo.TimeZone = userInfo.TimeZone;
                    currentUserInfo.Language = userInfo.Language;
                    currentUserInfo.Updated = DateTime.UtcNow;

                    _context.UserInfos.Update(currentUserInfo);
                    await _context.SaveChangesAsync();

                    response.Data = currentUserInfo;
                    return response;
                }
            }

            response.Success = false;
            response.Message = "Error: Invalid user data";
            return response;
        }
    }
}
```
<br/>

### Register UserInfosService for dependency injection

Open Examplium.Server/Program.cs, go to the line just above `var app = builder.Build();` 

Add the UserInfosService service:
```
builder.Services.AddScoped<IUserInfosService, UserInfosService>();
```

<br/>
