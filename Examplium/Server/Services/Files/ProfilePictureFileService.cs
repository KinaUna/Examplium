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
