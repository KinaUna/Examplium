# Add Notes Service in Examplium.Client

This service depends on the ServiceResponse class, if it hasn't been created yet, see: https://github.com/KinaUna/Examplium/blob/master/Documentation/Code/Common/01-ServiceResponse.md

<br/>

### Add folders
In the Examplium.Client root add a new folder named "Services".

In the Services folder add a folder with the name "Notes".

<br/>

### Add INotesService.cs interface

In the Notes folder add a new interface file with the name "INotesService.cs" and replace the content with this:
```
using Examplium.Shared.Models.Domain;

namespace Examplium.Client.Services.Notes
{
    public interface INotesService
    {
        event Action OnChange;

        List<Note> MyNotes { get; set; }

        Task CreateNote(Note note);
        Task UpdateNote(Note note);
        Task DeleteNote(Note note);
        Task<Note?> GetNoteById(int id);
        Task GetMyNotes();
    }
}
```

<br/>

### Add NotesService.cs
In the Notes folder add a new class file with the name "NotesService.cs" and replace the content with this:
```
using System.Net.Http.Json;
using Examplium.Shared.Models.Domain;
using Examplium.Shared.Models.Services;

namespace Examplium.Client.Services.Notes
{
    public class NotesService: INotesService
    {
        private readonly HttpClient _httpClient;

        public NotesService(HttpClient httpClient)
        {
            _httpClient = httpClient;
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
                var deleteNoteResult = await response.Content.ReadFromJsonAsync<ServiceResponse<Note>>();
                if (deleteNoteResult != null && deleteNoteResult.Success)
                {
                    await GetMyNotes();
                    OnChange?.Invoke();
                    return deleteNoteResult.Data;
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

### Register NotesService for dependency injection

Open Examplium.Server/Program.cs, go to the line just above `await builder.Build().RunAsync();` 

Add the NotesService service:
```
builder.Services.AddScoped<INotesService, NotesService>();
```

<br/>
