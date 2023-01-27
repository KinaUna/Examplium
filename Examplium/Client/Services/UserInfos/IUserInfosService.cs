using Examplium.Shared.Models.Domain;

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
