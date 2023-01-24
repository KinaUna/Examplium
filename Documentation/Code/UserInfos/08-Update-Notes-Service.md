# Update the Client Notes service

To display time and date information correctly each Note's Updated and Created properties need to be converted to the current users time zone.

The conversion uses this TimeZoneConverter package: https://github.com/mattjohnsonpint/TimeZoneConverter

<br/>

### Install TimeZoneConverter NuGet package

Right click on the Examplium.Client projects, select "Manage Nuget Packages..".

Go to the "Browse" tab and enter "TimeZoneConverter" in the search field.

Install TimeZoneConverter by Matt Johnson-Pint, currently it is vertion 6.0.1.

<br/>

### Update Examplium.Client NotesService

Open the Examplium.Client/Services/Notes/NotesService.cs file.

Change the code to the following:
```
using System.Net.Http.Json;
using Examplium.Client.Services.UserInfos;
using Examplium.Shared.Models.Domain;
using Examplium.Shared.Models.Services;
using TimeZoneConverter;

namespace Examplium.Client.Services.Notes
{
    public class NotesService: INotesService
    {
        private readonly HttpClient _httpClient;
        private readonly IUserInfosService _userInfosService;

        public NotesService(HttpClient httpClient, IUserInfosService userInfosService)
        {
            _httpClient = httpClient;
            _userInfosService = userInfosService;
        }

        public event Action? OnChange;

        public List<Note> MyNotes { get; set; } = new List<Note>();

        public async Task CreateNote(Note note)
        {
            var response = await _httpClient.PostAsJsonAsync("api/Notes/AddNote", note);
            if (response.IsSuccessStatusCode)
            {
                var createdNoteResult = await response.Content.ReadFromJsonAsync<ServiceResponse<Note>>();
                if (createdNoteResult != null && createdNoteResult.Success)
                {
                    await GetMyNotes();
                    OnChange?.Invoke();
                }
            }

            // Todo: Error message if result.Success or response.IsSuccessStatusCode is false
        }

        public async Task UpdateNote(Note note)
        {
            var response = await _httpClient.PostAsJsonAsync("api/Notes/UpdateNote", note);
            if (response.IsSuccessStatusCode)
            {
                var updateNoteResult = await response.Content.ReadFromJsonAsync<ServiceResponse<Note>>();
                if (updateNoteResult != null && updateNoteResult.Success)
                {
                    await GetMyNotes();
                    OnChange?.Invoke();
                }
            }

            // Todo: Error message if result.Success or response.IsSuccessStatusCode is false
        }

        public async Task DeleteNote(Note note)
        {
            var response = await _httpClient.PostAsJsonAsync("api/Notes/DeleteNote", note);
            if (response.IsSuccessStatusCode)
            {
                var deleteNoteResult = await response.Content.ReadFromJsonAsync<ServiceResponse<Note>>();
                if (deleteNoteResult != null && deleteNoteResult.Success)
                {
                    await GetMyNotes();
                    OnChange?.Invoke();
                }
            }

            // Todo: Error message if result.Success or response.IsSuccessStatusCode is false
        }

        public async Task<Note?> GetNoteById(int id)
        {
            var response = await _httpClient.PostAsJsonAsync("api/Notes/GetNoteById", id);
            if (response.IsSuccessStatusCode)
            {
                var getNoteResult = await response.Content.ReadFromJsonAsync<ServiceResponse<Note>>();
                if (getNoteResult != null && getNoteResult.Success)
                {
                    if (_userInfosService.CurrentUser == null)
                    {
                        await _userInfosService.GetCurrentUserInfo();
                    }

                    if (getNoteResult.Data != null && !string.IsNullOrEmpty(_userInfosService.CurrentUser?.TimeZone))
                    {
                        TimeZoneInfo timeZoneInfo = TZConvert.GetTimeZoneInfo(_userInfosService.CurrentUser.TimeZone);

                        getNoteResult.Data.Created = TimeZoneInfo.ConvertTimeFromUtc(getNoteResult.Data.Created, timeZoneInfo);
                        getNoteResult.Data.Updated = TimeZoneInfo.ConvertTimeFromUtc(getNoteResult.Data.Updated, timeZoneInfo);
                    }
                    

                    await GetMyNotes();
                    OnChange?.Invoke();
                    return getNoteResult.Data;
                }
            }

            // Todo: Error message if result.Success or response.IsSuccessStatusCode is false

            return null;
        }

        public async Task GetMyNotes()
        {
            var response = await _httpClient.GetAsync("api/Notes/GetMyNotes");
            if (response.IsSuccessStatusCode)
            {
                var getMyNotesList = await response.Content.ReadFromJsonAsync<ServiceResponse<List<Note>>>();
                if (getMyNotesList != null && getMyNotesList.Success && getMyNotesList.Data != null)
                {
                    if (_userInfosService.CurrentUser == null)
                    {
                        await _userInfosService.GetCurrentUserInfo();
                    }

                    if (!string.IsNullOrEmpty(_userInfosService.CurrentUser?.TimeZone))
                    {
                        TimeZoneInfo timeZoneInfo = TZConvert.GetTimeZoneInfo(_userInfosService.CurrentUser.TimeZone);
                        foreach (var note in getMyNotesList.Data)
                        {
                            note.Created = TimeZoneInfo.ConvertTimeFromUtc(note.Created, timeZoneInfo);
                            note.Updated = TimeZoneInfo.ConvertTimeFromUtc(note.Updated, timeZoneInfo);
                        }
                    }
                    

                    MyNotes = getMyNotesList.Data;
                }
                else
                {
                    MyNotes = new List<Note>();
                }
            }
        }
    }
}
```

<br/>
