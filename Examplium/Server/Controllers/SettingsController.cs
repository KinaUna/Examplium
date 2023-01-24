using Examplium.Server.Services.Settings;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Examplium.Server.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class SettingsController : ControllerBase
    {
        private readonly ITimezonesService _timezonesService;
        public SettingsController(ITimezonesService timezonesService)
        {
            _timezonesService = timezonesService;
        }

        [HttpGet("GetAllTimezones")]
        public ActionResult GetAllTimezones()
        {
            var allTimeZonesResponse = _timezonesService.GetAllTimeZones();

            return Ok(allTimeZonesResponse);
        }
        
    }
}
