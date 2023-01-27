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
