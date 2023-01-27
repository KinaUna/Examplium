using Examplium.Shared.Models.DTOs;
using Examplium.Shared.Models.Services;
using System.Net.Http.Json;

namespace Examplium.Client.Services.Settings
{
    public class TimezonesService: ITimezonesService
    {
        private readonly HttpClient _httpClient;

        public TimezonesService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        private List<TimeZoneDTO> _timezonesList = new List<TimeZoneDTO>();

        public async Task<List<TimeZoneDTO>> GetAllTimeZones()
        {
            if (!_timezonesList.Any())
            {
                var response = await _httpClient.GetAsync("api/Settings/GetAllTimezones");
                if (response.IsSuccessStatusCode)
                {
                    var allTimezonesResponse = await response.Content.ReadFromJsonAsync<ServiceResponse<List<TimeZoneDTO>>>();
                    if (allTimezonesResponse?.Data != null && allTimezonesResponse.Success)
                    {
                        _timezonesList = allTimezonesResponse.Data;
                    }
                }
            }
            
            return _timezonesList;
        }
    }
}
