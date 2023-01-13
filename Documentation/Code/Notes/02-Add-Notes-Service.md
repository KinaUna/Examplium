# Add Notes Service

### Add folders

In the Examplium.Server root add a new folder named "Services".

In the Services folder add a folder with the name "Notes".

<br/>

### Add INotesService.cs interface

In the Notes folder add a new interface file with the name "INotesService.cs" and replace the content with this:
```
using Examplium.Shared.Models.Domain;

namespace Examplium.Server.Services.Notes
{
    public interface INotesService
    {
        Task<Note?> CreateNote(Note note);
        Task<Note?> UpdateNote(Note note);
        Task<Note?> DeleteNote(Note note);
        Task<Note?> GetNoteById(int id);
    }
}
```

<br/>

### Add NotesService.cs
In the Notes folder add a new class file with the name "NotesService.cs" and replace the content with this:
```
using Examplium.Server.Data;
using Examplium.Server.Services.Auth;
using Examplium.Shared.Models.Domain;
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

        public async Task<Note?> CreateNote(Note note)
        {
            string? userId = _authService.GetUserId();
            if (userId != null)
            {
                note.Author = userId;
                _ = await _context.Notes.AddAsync(note);
                _ = await _context.SaveChangesAsync();

                return note;
            }

            return null;
        }

        public async Task<Note?> UpdateNote(Note note)
        {
            Note? noteToUpdate = await _context.Notes.SingleOrDefaultAsync(n => n.Id == note.Id);

            if (noteToUpdate != null && noteToUpdate.Author == _authService.GetUserId())
            {
                noteToUpdate.Title = noteToUpdate.Title;
                noteToUpdate.Content = noteToUpdate.Content;
                noteToUpdate.Category = note.Category;
                noteToUpdate.Tags = note.Tags;
                noteToUpdate.Updated = DateTime.UtcNow;

                _ = _context.Notes.Update(noteToUpdate);
                _ = await _context.SaveChangesAsync();

                return noteToUpdate;
            }

            return null;
        }

        public async Task<Note?> DeleteNote(Note note)
        {
            Note? noteToDelete = await _context.Notes.SingleOrDefaultAsync(n => n.Id == note.Id);
            if (noteToDelete != null && note.Author == _authService.GetUserId())
            {
                _context.Notes.Remove(noteToDelete);
                _ = await _context.SaveChangesAsync();
                return noteToDelete;
            }

            return null;

        }

        public async Task<Note?> GetNoteById(int id)
        {
            Note? noteResult = await _context.Notes.SingleOrDefaultAsync(n => n.Id == id);

            return noteResult;
        }
    }
}
```
<br/>

### Register NotesService for dependency injection

Open Examplium.Server/Program.cs, go to the line just above `var app = builder.Build();` 

Add the NotesService service:
```
builder.Services.AddScoped<INotesService, NotesService>();
```

<br/>
