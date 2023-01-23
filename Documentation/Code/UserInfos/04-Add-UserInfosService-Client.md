# Add UserInfos Service in Examplium.Client

This service depends on the ServiceResponse class, if it hasn't been created yet, see: https://github.com/KinaUna/Examplium/blob/master/Documentation/Code/Common/01-ServiceResponse.md

<br/>

### Add folders
In the Examplium.Client root add a new folder named "Services", if it doesn't exist.

In the Services folder add a folder with the name "UserInfos".

<br/>

### Add IUserInfosService.cs interface

In the UserInfos folder add a new interface file with the name "IUserInfosService.cs" and replace the content with this:
```
namespace Examplium.Client.Services.UserInfos
{
    public interface IUserInfosService
    {
        event Action? OnChange;
        UserInfo? CurrentUser { get; set; }
        Task GetCurrentUserInfo();
        Task UpdateCurrentUser();
    }
}
```

<br/>

### Add UserInfosService.cs
In the UserInfos folder add a new class file with the name "UserInfosService.cs" and replace the content with this:
```
using System.Net.Http.Json;
using Examplium.Shared.Models.Domain;
using Examplium.Shared.Models.Services;

namespace Examplium.Client.Services.UserInfos
{
    public class UserInfosService: IUserInfosService
    {
        private readonly HttpClient _httpClient;

        public UserInfosService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public event Action? OnChange;

        public UserInfo? CurrentUser { get; set; }

        public async Task GetCurrentUserInfo()
        {
            var response = await _httpClient.GetAsync("api/UserInfos");
            if (response.IsSuccessStatusCode)
            {
                var currentUserInfoResponse = await response.Content.ReadFromJsonAsync <ServiceResponse<UserInfo>>();
                if (currentUserInfoResponse?.Data != null && currentUserInfoResponse.Success)
                {
                    CurrentUser = currentUserInfoResponse.Data;
                    OnChange?.Invoke();
                }
            }
        }

        public async Task UpdateCurrentUser()
        {
            var response = await _httpClient.PostAsJsonAsync("api/UserInfos/UpdateCurrentUser", CurrentUser);
            if (response.IsSuccessStatusCode)
            {
                var updateUserInfoResponse = await response.Content.ReadFromJsonAsync<ServiceResponse<UserInfo>>();
                if (updateUserInfoResponse?.Data != null && updateUserInfoResponse.Success)
                {
                    CurrentUser = updateUserInfoResponse.Data;
                    OnChange?.Invoke();
                }
            }
        }
    }
}
```

<br/>

### Register UserInfosService for dependency injection

Open Examplium.Server/Program.cs, go to the line just above `await builder.Build().RunAsync();` 

Add the UserInfosService service:
```
builder.Services.AddScoped<IUserInfosService, UserInfosService>();
```

<br/>
