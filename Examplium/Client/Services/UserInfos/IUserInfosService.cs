namespace Examplium.Client.Services.UserInfos
{
    public interface IUserInfosService
    {
        event Action? OnChange;
        Task GetCurrentUserInfo();
        Task UpdateCurrentUser();
    }
}
