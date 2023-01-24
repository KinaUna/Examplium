using Examplium.Shared.Models.DTOs;

namespace Examplium.Client.Services.Settings
{
    public interface ITimezonesService
    {
        Task<List<TimeZoneDTO>> GetAllTimeZones();
    }
}
