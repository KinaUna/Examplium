# Add Profile Picture

<br/>

### Add Model for file uploads

If it doesn't exist create a new folder in Examplium.Shared/Models/ and name it "Services".

In the Services folder add a new class file and give it the name "UploadFileResponse.cs".

Add this code to the new file:
```
namespace Examplium.Shared.Models.Services
{
    public class UploadFileResponse
    {
        public bool Success { get; set; }
        public string? FileName { get; set; }
        public string? SavedFileName { get; set; }
        public string Message { get; set; } = string.Empty;
    }
}
```

<br/>

### Add constant for upload maximum file size

In the Examplium.Shared/Constants/ExampliumCoreConstants.cs file add this line:
```
public const long ProfilePictureMaxFileSize = 1024 * 1024 * 3; // 3MB
```

<br/>

### Add ProfilePictureService for Server

If it doesn't exist, add a new folder in Examplium.Server/Services named "Files".

Add a new interface file with the name "IProfilePictureFileService.cs", and add this code:
```
using Examplium.Shared.Models.Services;

namespace Examplium.Server.Services.Files
{
    public interface IProfilePictureFileService
    {
        Task<UploadFileResponse> SaveProfilePicture(IEnumerable<IFormFile> pictureFiles);
        Task<string> GetProfilePicturePath(string savedFileName);
    }
}
```

<br/>

Add a class file and name it "ProfilePictureFileService.cs".

Code:
```
using Examplium.Shared.Models.Services;
using System.Net;
using Examplium.Server.Services.Auth;
using Examplium.Server.Services.UserInfos;
using Examplium.Shared.Constants;

namespace Examplium.Server.Services.Files
{
    public class ProfilePictureFileService: IProfilePictureFileService
    {
        private readonly IUserInfosService _userInfosService;
        private readonly string _uploadDirectory;
        private readonly string _assetsDirectory;

        public ProfilePictureFileService(IWebHostEnvironment webHostEnvironment, IAuthService authService, IUserInfosService userInfosService)
        {
            _userInfosService = userInfosService;
            _uploadDirectory = Path.Combine(webHostEnvironment.ContentRootPath, "wwwUserFiles", webHostEnvironment.EnvironmentName);
            _assetsDirectory = Path.Combine(webHostEnvironment.ContentRootPath, "Assets");
        }
        public async Task<UploadFileResponse> SaveProfilePicture(IEnumerable<IFormFile> pictureFiles)
        {
            
            UploadFileResponse uploadFileResponse = new UploadFileResponse();

            IFormFile? pictureFile = pictureFiles.FirstOrDefault();
            if (pictureFile == null)
            {
                uploadFileResponse.Success = false;
                uploadFileResponse.Message = "Error: File content not found.";
                return uploadFileResponse;
            }

            string untrustedFileName = pictureFile.FileName;
            uploadFileResponse.FileName = untrustedFileName;
            string trustedFileNameForDisplay = WebUtility.HtmlEncode(untrustedFileName);

            string? userInfoId = (await _userInfosService.GetCurrentUserInfo()).Data?.Id.ToString();
            if (string.IsNullOrEmpty(userInfoId))
            {
                uploadFileResponse.Success = false;
                uploadFileResponse.Message = "Error: Invalid User Id";
                return uploadFileResponse;
            }

            if (pictureFile.Length == 0)
            {
                uploadFileResponse.Success = false;
                uploadFileResponse.Message = $"Error: {trustedFileNameForDisplay} has no content.";
                return uploadFileResponse;
            }
            
            if (pictureFile.Length > ExampliumCoreConstants.ProfilePictureMaxFileSize)
            {
                uploadFileResponse.Success = false;
                uploadFileResponse.Message = $"Error: {trustedFileNameForDisplay} is larger than the maximum allowed size.";
                return uploadFileResponse;
            }

            try
            {
                string trustedFileNameForFileStorage = Path.ChangeExtension(Path.GetRandomFileName(), Path.GetExtension(pictureFile.FileName));

                var profilePicturesDirectory = Path.Combine(_uploadDirectory, userInfoId, "Profile");
                if (!Directory.Exists(profilePicturesDirectory))
                {
                    Directory.CreateDirectory(profilePicturesDirectory);
                }

                var path = Path.Combine(_uploadDirectory, userInfoId, "Profile", trustedFileNameForFileStorage);

                await using FileStream pictureFileStream = new(path, FileMode.Create);
                await pictureFile.CopyToAsync(pictureFileStream);

                uploadFileResponse.Success = true;
                uploadFileResponse.SavedFileName = trustedFileNameForFileStorage;
            }
            catch (IOException ex)
            {
                uploadFileResponse.Success = false;
                uploadFileResponse.Message = $"Error uploading file {trustedFileNameForDisplay}: {ex.Message}";
                
            }

            return uploadFileResponse;
        }

        public async Task<string> GetProfilePicturePath(string savedFileName)
        {
            string path = Path.Combine(_assetsDirectory, "DefaultProfilePicture.jpg");
            string ? userInfoId = (await _userInfosService.GetCurrentUserInfo()).Data?.Id.ToString();
            if (!string.IsNullOrEmpty(savedFileName) && !string.IsNullOrEmpty(userInfoId))
            {
                string userProfilePicturePath = Path.Combine(_uploadDirectory, userInfoId, "Profile", savedFileName);
                if (File.Exists(userProfilePicturePath))
                {
                    path = userProfilePicturePath;
                }
            }

            return path;
        }
    }
}
```

<br/>

Register the service for dependency injection.

Open Examplium.Server/Program.cs, go to the line just above var app = builder.Build(); and add this:
```
builder.Services.AddScoped<IProfilePictureFileService, ProfilePictureFileService>();
```

<br/>

### Update UserInfosController

Open the Examplium.Server/Controllers/UserInfosController.cs file.

Replace the content with this code:
```
using Duende.Bff;
using Examplium.Server.Services.Files;
using Examplium.Server.Services.UserInfos;
using Examplium.Shared.Models.Domain;
using Examplium.Shared.Models.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;

namespace Examplium.Server.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UserInfosController : ControllerBase
    {
        private readonly IUserInfosService _userInfosService;
        private readonly IProfilePictureFileService _profilePictureFileService;

        public UserInfosController(IUserInfosService userInfosService, IProfilePictureFileService profilePictureFileService)
        {
            _userInfosService = userInfosService;
            _profilePictureFileService = profilePictureFileService;
        }

        [HttpGet]
        public async Task<ActionResult> Get()
        {
            var currentUserResponse = await _userInfosService.GetCurrentUserInfo();

            return Ok(currentUserResponse);
        }
        
        [HttpPost("UpdateCurrentUser")]
        public async Task<ActionResult> UpdateCurrentUser(UserInfo userInfo)
        {
            var updateCurrentUserResponse = await _userInfosService.UpdateCurrentUserInfo(userInfo);

            return Ok(updateCurrentUserResponse);
        }

        [HttpPost("UploadProfilePicture")]
        public async Task<ActionResult> UploadProfilePicture([FromForm] IEnumerable<IFormFile> pictureFiles)
        {
            UploadFileResponse response = await _profilePictureFileService.SaveProfilePicture(pictureFiles);
            return Ok(response);
        }

        [BffApiSkipAntiforgeryAttribute]
        [ResponseCache(Location = ResponseCacheLocation.Client, NoStore = true)]
        [HttpGet("ProfilePicture/{fileName}")]
        public async Task<ActionResult> GetProfilePicture(string fileName)
        {
            string filePath = await _profilePictureFileService.GetProfilePicturePath(fileName);
            var fileExensionsTypeProvider = new FileExtensionContentTypeProvider();

            if (!fileExensionsTypeProvider.TryGetContentType(fileName, out string? contentType))
            {
                contentType = "application/octet-stream";
            }

            return PhysicalFile(filePath, contentType);
        }
    }
}
```

<br/>

### Add image file for default profile picture.

Create a new folder in Examplium.Server and name it "Assets".

Then copy a picture file with the placeholder/default image for profile picture and name it "DefaultProfilePicture.jpg".
(or update the file name in the method GetProfilePicturePath in ProfilePictureFileService.cs to match the file copied)

<br/>

### Add UserProfilePictureService for Client

If it doesn't exist, add a new folder in Examplium.Server/Services named "UserInfos".



Add a new interface file with the name "IUserProfilePictureService.cs", and add this code:
```
using Examplium.Shared.Models.Services;
using Microsoft.AspNetCore.Components.Forms;

namespace Examplium.Client.Services.UserInfos
{
    public interface IUserProfilePictureService
    {
        Task<UploadFileResponse> UploadProfilePicture(IBrowserFile pictureFile);
        string GetProfilePictureUrl(string fileName);
    }
}
```

<br/>

Add a class file and name it "UserProfilePictureService.cs".

Code:
```
using Examplium.Shared.Constants;
using Examplium.Shared.Models.Services;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Components.Forms;

namespace Examplium.Client.Services.UserInfos
{
    public class UserProfilePictureService: IUserProfilePictureService
    {
        private readonly HttpClient _httpClient;

        public UserProfilePictureService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<UploadFileResponse> UploadProfilePicture(IBrowserFile pictureFile)
        {
            using var content = new MultipartFormDataContent();


            var fileContent = new StreamContent(pictureFile.OpenReadStream(ExampliumCoreConstants.ProfilePictureMaxFileSize));

            fileContent.Headers.ContentType = new MediaTypeHeaderValue(pictureFile.ContentType);

            content.Add(
                content: fileContent,
                name: "\"pictureFiles\"",
                fileName: pictureFile.Name);
            
            var response = await _httpClient.PostAsync("api/UserInfos/UploadProfilePicture", content);

            if (response.IsSuccessStatusCode)
            {
                var updateUserInfoResponse = await response.Content.ReadFromJsonAsync<UploadFileResponse>();
                if (updateUserInfoResponse != null)
                {
                    return updateUserInfoResponse;
                }
                
            }

            return new UploadFileResponse { Success = false, Message = response.ReasonPhrase?? "Error uploading file." + " " + response.RequestMessage };
        }

        public string GetProfilePictureUrl(string fileName)
        {
            string pictureUrl = _httpClient.BaseAddress!.AbsoluteUri + "api/UserInfos/ProfilePicture/" + fileName;

            return pictureUrl;
        }
    }
}
```

<br/>

Register the service for dependency injection.

Open Examplium.Server/Program.cs, go to the line just above var app = builder.Build().RunAsync(); and add this:
```
builder.Services.AddScoped<IUserProfilePictureService, UserProfilePictureService>();
```

<br/>

### Update UserInfoDetails.razor

Open the file Examplium.Client/Shared/Components/UserInfos/UserInfoDetails.razor

Replace the content with this code:
```
@using Examplium.Client.Services.UserInfos
@using Examplium.Client.Shared.Components.Settings
@using Examplium.Shared.Constants
@using Microsoft.AspNetCore.Authorization
@using System.Net.Http.Headers
@using Examplium.Shared.Models.Services
@attribute [Authorize]
@inject IUserInfosService UserInfosService
@inject IUserProfilePictureService UserProfilePictureService
@implements IDisposable
@if (_editUserInfo && UserInfosService.CurrentUser != null)
{
    <EditForm Model="UserInfosService.CurrentUser" OnSubmit="UpdateUserInfo">
        <div class="card card bg-userinfo rounded">
            <div class="card-header bg-userinfo-header text-white">
                @UserInfosService.CurrentUser.Email
            </div>
            <div class="card-body">
                <label class="text-white-50" for="firstNameInput">First name</label>
                <InputText id="firstNameInput" class="form-control mb-3" @bind-Value="UserInfosService.CurrentUser.FirstName" DisplayName="First name"></InputText>
                <label class="text-white-50" for="middleNameInput">Middle name</label>
                <InputText id="middleNameInput" class="form-control mb-3" @bind-Value="UserInfosService.CurrentUser.MiddleName" DisplayName="Middle name"></InputText>
                <label class="text-white-50" for="lastNameInput">Last name</label>
                <InputText id="lastNameInput" class="form-control mb-3" @bind-Value="UserInfosService.CurrentUser.LastName" DisplayName="Last name"></InputText>
                <label class="text-white-50">Timezone</label>
                <div class="form-control mb-3">
                    <TimezoneSelector @bind-SelectedTimezone="UserInfosService.CurrentUser.TimeZone"></TimezoneSelector>
                </div>
                <label class="text-white-50">Profile picture</label>
                @if (!string.IsNullOrEmpty(UserInfosService.CurrentUser?.Picture))
                {
                    <div><img src="@UserProfilePictureService.GetProfilePictureUrl(UserInfosService.CurrentUser.Picture)" height="150px"/></div>
                }
                <InputFile class="form-control mb-3" accept=".png, .jpg, .jpeg" OnChange="@OnProfilePictureChange" />
            </div>
            <div class="card-footer">
                <div class="mt-3 w-100">
                    <div class="float-end">
                        <button class="btn btn-success" type="submit"><span class="oi oi-circle-check" aria-hidden="true"></span> Save</button>
                        <button class="btn btn-secondary" @onclick="CancelEdit"><span class="oi oi-circle-x" aria-hidden="true"></span> Cancel</button>
                    </div>
                </div>
            </div>
        </div>
    </EditForm>
}
else
{
    <div class="card card bg-userinfo rounded">
        <div class="card-header bg-userinfo-header">
            <div class="float-end">
                <button class="btn btn-primary ms-2" @onclick="EditUserInfo"><span class="oi oi-pencil" aria-hidden="true"></span></button>
            </div>
            <h4 class="text-white">@UserInfosService.CurrentUser?.Email</h4>
        </div>
        <div class="card-body text-white">
            <div class="row mb-3">
                <div class="col-3 col-lg-2 text-white-50">First name:</div>
                <div class="col-8"> @UserInfosService.CurrentUser?.FirstName</div>
            </div>
            <div class="row mb-3">
                <div class="col-3 col-lg-2 text-white-50">Middle name: </div>
                <div class="col-8">@UserInfosService.CurrentUser?.MiddleName</div>
            </div>
            <div class="row mb-3">
                <div class="col-3 col-lg-2 text-white-50">Last name:</div>
                <div class="col-8">@UserInfosService.CurrentUser?.LastName</div>
            </div>
            <div class="row mb-3">
                <div class="col-3 col-lg-2 text-white-50">Time zone:</div>
                <div class="col-8">@UserInfosService.CurrentUser?.TimeZone</div>
            </div>
            @if (!string.IsNullOrEmpty(UserInfosService.CurrentUser?.Picture))
            {
                <div class="row mb-3">
                    <div class="col-3 col-lg-2 text-white-50">Profile picture:</div>
                    <div class="col-8"> <img src="@UserProfilePictureService.GetProfilePictureUrl(UserInfosService.CurrentUser.Picture)" height="150px" /></div>
                </div>
            }
        </div>
    </div>
}
@if (!string.IsNullOrEmpty(_errorMessage))
{
    <div class="bg-danger">
        <span class="text-white">Error: @_errorMessage</span>
    </div>
}
@code {
    private bool _editUserInfo = false;
    private bool _profilePictureChanged = false;
    private IBrowserFile? _uploadPictureFile;
    private string _errorMessage = string.Empty;
    protected override async Task OnInitializedAsync()
    {
        await UserInfosService.GetCurrentUserInfo();
        UserInfosService.OnChange += StateHasChanged;
    }

    public void Dispose()
    {
        UserInfosService.OnChange -= StateHasChanged;
    }

    private async Task EditUserInfo()
    {
        _errorMessage = string.Empty;
        _editUserInfo = true;
    }

    private async Task CancelEdit()
    {
        _editUserInfo = false;
        _profilePictureChanged = false;
        _uploadPictureFile = null;

        await UserInfosService.GetCurrentUserInfo();
    }

    private async Task UpdateUserInfo()
    {
        _errorMessage = string.Empty;
        if (UserInfosService.CurrentUser != null)
        {
            if (_profilePictureChanged && _uploadPictureFile != null)
            {
                Console.WriteLine("Upload picture file: " + _uploadPictureFile.Name);
                UploadFileResponse uploadFileResponse = await UserProfilePictureService.UploadProfilePicture(_uploadPictureFile);
                if (uploadFileResponse.Success && !string.IsNullOrEmpty(uploadFileResponse.SavedFileName))
                {
                    UserInfosService.CurrentUser.Picture = uploadFileResponse.SavedFileName;
                }
                else
                {
                    _errorMessage = uploadFileResponse.Message;
                }
            }

            await UserInfosService.UpdateCurrentUser();
        }

        _editUserInfo = false;
    }

    private void OnProfilePictureChange(InputFileChangeEventArgs e)
    {
        _uploadPictureFile = e.File;
        _profilePictureChanged = true;

    }

}
```

<br/>

### References

File uploads for ASP.NET Core 7.0 WebAssembly: https://learn.microsoft.com/en-us/aspnet/core/blazor/file-uploads?view=aspnetcore-7.0&pivots=webassembly

Getting a Mime Type from a File Name in .NET Core: https://dotnetcoretutorials.com/2018/08/14/getting-a-mime-type-from-a-file-name-in-net-core/

Response caching: https://learn.microsoft.com/en-us/aspnet/core/performance/caching/response?view=aspnetcore-7.0#http-based-caching-respects-request-cache-control-directives
