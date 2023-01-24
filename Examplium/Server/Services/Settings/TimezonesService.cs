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
