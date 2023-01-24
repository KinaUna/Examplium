# Add Timezone functionality on Server.

<br/>

### Add Data Transfer Object model
Add a new folder in the Examplium.Shared/Models folder, name it "DTOs".

Add a new class file with the name "TimeZoneDTO.cs.

Code:
```
namespace Examplium.Shared.Models.DTOs
{
    public class TimeZoneDTO
    {
        public string Id { get; set; } = string.Empty;
        public string DisplayName { get; set; } = string.Empty;

    }
}
```

<br/>

### Add service on server.

If it doesn't exist, add a new folder in Examplium.Server/Services named "Settings".

Add a new interface file with the name "ITimezonesService.cs", and add this code:
```
using Examplium.Shared.Models.DTOs;
using Examplium.Shared.Models.Services;

namespace Examplium.Server.Services.Settings
{
    public interface ITimezonesService
    {
        ServiceResponse<List<TimeZoneDTO>> GetAllTimeZones();
    }
}
```

Add a class file and name it "TimezonesService.cs".

Code:
```
using Examplium.Shared.Models.Services;
using System.Collections.ObjectModel;
using Examplium.Shared.Models.DTOs;

namespace Examplium.Server.Services.Settings
{
    public class TimezonesService: ITimezonesService
    {
        public ServiceResponse<List<TimeZoneDTO>> GetAllTimeZones()
        {
            ReadOnlyCollection<TimeZoneInfo> systemTimeZones = TimeZoneInfo.GetSystemTimeZones();
            List<TimeZoneDTO> timeZoneDtos = new List<TimeZoneDTO>();
            foreach (TimeZoneInfo timeZone in systemTimeZones)
            {
                TimeZoneDTO timeZoneDto = new TimeZoneDTO();
                timeZoneDto.Id = timeZone.Id;
                timeZoneDto.DisplayName = timeZone.DisplayName;
                timeZoneDtos.Add(timeZoneDto);
            }

            ServiceResponse<List<TimeZoneDTO>> response = new ServiceResponse<List<TimeZoneDTO>>
            {
                Data = timeZoneDtos
            };

            return response;
        }
    }
}
```

Register the service for dependency injection.

Open Examplium.Server/Program.cs, go to the line just above `var app = builder.Build();` and add this:
```
builder.Services.AddScoped<ITimezonesService, TimezonesService>();
```

<br/>

### Add API Controller

Add an empty API Controller to the Examplium.Server/Controllers/ folder.

Name it "SettingsController.cs".

Code:
```
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
```

<br/>
