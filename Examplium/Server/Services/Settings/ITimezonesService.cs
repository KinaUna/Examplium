using Examplium.Shared.Models.DTOs;
using Examplium.Shared.Models.Services;

namespace Examplium.Server.Services.Settings
{
    public interface ITimezonesService
    {
        ServiceResponse<List<TimeZoneDTO>> GetAllTimeZones();
    }
}
