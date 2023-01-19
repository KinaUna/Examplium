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
