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
