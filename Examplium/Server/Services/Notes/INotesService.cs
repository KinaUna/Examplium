using Examplium.Shared.Models.Domain;
using Examplium.Shared.Models.Services;

namespace Examplium.Server.Services.Notes
{
    public interface INotesService
    {
        Task<ServiceResponse<Note>> CreateNote(Note note);
        Task<ServiceResponse<Note>> UpdateNote(Note note);
        Task<ServiceResponse<Note>> DeleteNote(Note note);
        Task<ServiceResponse<Note>> GetNoteById(int id);
        Task<ServiceResponse<List<Note>>> GetMyNotes();
    }
}
