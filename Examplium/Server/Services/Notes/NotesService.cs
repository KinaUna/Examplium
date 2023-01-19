using Examplium.Server.Data;
using Examplium.Server.Services.Auth;
using Examplium.Shared.Models.Domain;
using Examplium.Shared.Models.Services;
using Microsoft.EntityFrameworkCore;

namespace Examplium.Server.Services.Notes
{
    public class NotesService: INotesService
    {
        private readonly ApplicationDbContext _context;
        private readonly IAuthService _authService;

        public NotesService(ApplicationDbContext context, IAuthService authService)
        {
            _context = context;
            _authService = authService;
        }

        public async Task<ServiceResponse<Note>> CreateNote(Note note)
        {
            ServiceResponse<Note> response = new ServiceResponse<Note>();
            string? userId = _authService.GetUserId();
            if (userId != null)
            {
                note.Author = userId;
                _ = await _context.Notes.AddAsync(note);
                _ = await _context.SaveChangesAsync();

                response.Data = note;
            }
            else
            {
                response.Success = false;
                response.Message = "Invalid user data.";
            }
            
            return response;
        }

        public async Task<ServiceResponse<Note>> UpdateNote(Note note)
        {
            ServiceResponse<Note> response = new ServiceResponse<Note>();
            Note? noteToUpdate = await _context.Notes.SingleOrDefaultAsync(n => n.Id == note.Id);

            if (noteToUpdate != null && noteToUpdate.Author == _authService.GetUserId())
            {
                noteToUpdate.Title = note.Title;
                noteToUpdate.Content = note.Content;
                noteToUpdate.Category = note.Category;
                noteToUpdate.Tags = note.Tags;
                noteToUpdate.Updated = DateTime.UtcNow;

                _ = _context.Notes.Update(noteToUpdate);
                _ = await _context.SaveChangesAsync();

                response.Data = noteToUpdate;
            }
            else
            {
                response.Success = false;
                response.Message = "Invalid user data.";

                if (noteToUpdate == null)
                {
                    response.Message = "Invalid Note Id.";
                }
            }

            return response;
        }

        public async Task<ServiceResponse<Note>> DeleteNote(Note note)
        {
            ServiceResponse<Note> response = new ServiceResponse<Note>();
            Note? noteToDelete = await _context.Notes.SingleOrDefaultAsync(n => n.Id == note.Id);
            
            if (noteToDelete != null && note.Author == _authService.GetUserId())
            {
                _context.Notes.Remove(noteToDelete);
                _ = await _context.SaveChangesAsync();

                response.Data = noteToDelete;
            }
            else
            {
                response.Success = false;
                response.Message = "Invalid user data.";

                if (noteToDelete == null)
                {
                    response.Message = "Invalid Note Id.";
                }
            }
            return response;

        }

        public async Task<ServiceResponse<Note>> GetNoteById(int id)
        {
            ServiceResponse<Note> response = new ServiceResponse<Note>();

            Note? noteResult = await _context.Notes.SingleOrDefaultAsync(n => n.Id == id);

            if (noteResult != null && noteResult.Author == _authService.GetUserId())
            {
                response.Data = noteResult;
            }
            else
            {
                response.Success = false;
                response.Message = "Invalid usser data";
                if (noteResult == null)
                {
                    response.Message = "Invalid Note Id.";
                }
            }
            return response;
        }

        public async Task<ServiceResponse<List<Note>>> GetMyNotes()
        {
            ServiceResponse<List<Note>> response = new ServiceResponse<List<Note>>();

            List<Note> myNotes = await _context.Notes.Where(n => n.Author == _authService.GetUserId()).ToListAsync();
            
            response.Data = myNotes;

            return response;
        }
    }
}
