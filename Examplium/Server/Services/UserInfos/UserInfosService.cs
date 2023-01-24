using Examplium.Server.Data;
using Examplium.Server.Services.Auth;
using Examplium.Shared.Models.Domain;
using Examplium.Shared.Models.Services;
using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;

namespace Examplium.Server.Services.UserInfos
{
    public class UserInfosService: IUserInfosService
    {
        private readonly ApplicationDbContext _context;
        private readonly IAuthService _authService;

        public UserInfosService(ApplicationDbContext context, IAuthService authService)
        {
            _context = context;
            _authService = authService;
        }

        public async Task<ServiceResponse<UserInfo>> GetCurrentUserInfo()
        {
            ServiceResponse<UserInfo> response = new ServiceResponse<UserInfo>();

            string? currentUserId = _authService.GetUserId();

            if (!string.IsNullOrEmpty(currentUserId))
            {
                UserInfo? userInfo = await _context.UserInfos.SingleOrDefaultAsync(u => u.UserId == currentUserId);

                if (userInfo == null)
                {
                    userInfo = new UserInfo();
                    userInfo.UserId = currentUserId;
                    userInfo.Email = _authService.GetUserEmail()!;
                    userInfo.Updated = DateTime.UtcNow;
                    userInfo.Created = DateTime.UtcNow;

                    await _context.UserInfos.AddAsync(userInfo);
                    await _context.SaveChangesAsync();
                }

                response.Data = userInfo;
                return response;
            }

            response.Success = false;
            response.Message = "Error: Invalid user data";
            return response;
        }

        public async Task<ServiceResponse<UserInfo>> UpdateCurrentUserInfo(UserInfo userInfo)
        {
            ServiceResponse<UserInfo> response = new ServiceResponse<UserInfo>();

            string? currentUserId = _authService.GetUserId();
            
            if (!string.IsNullOrEmpty(currentUserId) && userInfo.UserId == currentUserId)
            {
                UserInfo? currentUserInfo = await _context.UserInfos.SingleOrDefaultAsync(u => u.UserId == currentUserId);
                if (currentUserInfo != null)
                {
                    currentUserInfo.FirstName = userInfo.FirstName;
                    currentUserInfo.MiddleName = userInfo.MiddleName;
                    currentUserInfo.LastName = userInfo.LastName;
                    currentUserInfo.Picture = userInfo.Picture;
                    currentUserInfo.TimeZone = userInfo.TimeZone;
                    currentUserInfo.Language = userInfo.Language;
                    currentUserInfo.Updated = DateTime.UtcNow;

                    _context.UserInfos.Update(currentUserInfo);
                    await _context.SaveChangesAsync();

                    response.Data = currentUserInfo;
                    return response;
                }
            }

            response.Success = false;
            response.Message = "Error: Invalid user data";
            return response;
        }
    }
}
