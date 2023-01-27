using Examplium.Shared.Models.Services;

namespace Examplium.Server.Services.Files
{
    public interface IProfilePictureFileService
    {
        Task<UploadFileResponse> SaveProfilePicture(IEnumerable<IFormFile> pictureFiles);
        Task<string> GetProfilePicturePath(string savedFileName);
    }
}
