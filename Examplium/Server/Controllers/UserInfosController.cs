using Examplium.Server.Services.Notes;
using Examplium.Server.Services.UserInfos;
using Examplium.Shared.Models.Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Examplium.Server.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UserInfosController : ControllerBase
    {
        private readonly IUserInfosService _userInfosService;
        public UserInfosController(IUserInfosService userInfosService)
        {
            _userInfosService = userInfosService;
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
    }
}
