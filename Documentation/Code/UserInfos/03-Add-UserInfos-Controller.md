# Add UserInfos Api Controller

Note: All the actions receiving data in this API use HttpPost even though HttpPut or HttpDelete would normally be used. 
This is done to avoid leaking information on the network in the url of the request.

It probably isn't important for most data, but doing it as a principle throughout this application minimizes the risk of revealing personal information in the URL unintentionally.

<br/>

### Add Controller class file

Add an empty API Controller to the Examplium.Server/Controllers/ folder.

Name it "UserInfosController.cs".

Code:
```
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
```
