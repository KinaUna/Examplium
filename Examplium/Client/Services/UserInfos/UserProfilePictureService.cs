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
