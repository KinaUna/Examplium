# Add Timezone functionality on Client.

<br/>

### Add service for Client.

If it doesn't exist, add a new folder in Examplium.Client/Services named "Settings".

Add a new interface file with the name "ITimezonesService.cs", and add this code:
```
using Examplium.Shared.Models.DTOs;

namespace Examplium.Client.Services.Settings
{
    public interface ITimezonesService
    {
        Task<List<TimeZoneDTO>> GetAllTimeZones();
    }
}
```

Add a class file and name it "TimezonesService.cs".
Code:
```
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
```

Register the service for dependency injection.
Open Examplium.Client/Program.cs, go to the line just above `var app = builder.Build();` and add this:
```
builder.Services.AddScoped<ITimezonesService, TimezonesService>();
```
<br/>

### Add TimezoneSelector razor component

In the Examplium.Client/Shared/Components/ folder add a new folder with the name "Settings".

Add a new Razor Component here and name it "TimezoneSelector.razor".

Code:
```
@using Examplium.Client.Services.Settings
@using Examplium.Shared.Models.DTOs
@inject ITimezonesService TimezoneService
<select class="form-select" @bind="SelectedTimezone">
    @foreach (var timeZoneItem in _timezonesList)
    {
        <option value="@timeZoneItem.Id">@timeZoneItem.Id: @timeZoneItem.DisplayName</option>
    }
</select>
@code {
    private string _selectedTimezone = "";

    [Parameter]
    public string? SelectedTimezone { 
        get => _selectedTimezone;
        set
        {
            if (_selectedTimezone != value && value != null)
            {
                _selectedTimezone = value;
                if (SelectedTimezoneChanged.HasDelegate)
                {
                    SelectedTimezoneChanged.InvokeAsync(value);
                }
            }
        }
    }

    [Parameter]
    public EventCallback<string> SelectedTimezoneChanged { get; set; }

    private List<TimeZoneDTO> _timezonesList = new List<TimeZoneDTO>();
    
    protected override async Task OnInitializedAsync()
    {
        _timezonesList = await TimezoneService.GetAllTimeZones();
        if (string.IsNullOrEmpty(SelectedTimezone))
        {
            SelectedTimezone = "UTC";
        }
    }

}
```

<br/>

### Add TimezoneSelector component to UserInfoDetails

Open the Examplium.Client/Shared/Components/UserInfos/UserInfoDetails.razor file.

After the InputText tage with the "lastNameInput" id add this code:
```
<label class="text-white-50">Timezone</label>
<div class="form-control mb-3">
   <TimezoneSelector @bind-SelectedTimezone="UserInfosService.CurrentUser.TimeZone"></TimezoneSelector>
</div>
```

<br/>
