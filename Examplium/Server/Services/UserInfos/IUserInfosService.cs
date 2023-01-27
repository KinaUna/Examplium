using Examplium.Shared.Models.Domain;
using Examplium.Shared.Models.Services;

namespace Examplium.Server.Services.UserInfos
{
    public interface IUserInfosService
    {
        Task<ServiceResponse<UserInfo>> GetCurrentUserInfo();
        Task<ServiceResponse<UserInfo>> UpdateCurrentUserInfo(UserInfo userInfo);
    }
}
