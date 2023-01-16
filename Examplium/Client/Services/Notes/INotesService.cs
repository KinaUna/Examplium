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
