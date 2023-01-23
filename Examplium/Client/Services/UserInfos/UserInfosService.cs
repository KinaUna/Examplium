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
