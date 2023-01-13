# Add Notes Api Controller

Note: All the actions in this API use HttpPost even though HttpPut or HttpDelete would normally be used. 
This is done to avoid leaking information on the network in the url of the request.

It probably isn't important for most data, but doing it as a principle throughout this application minimizes the risk of revealing personal information in the URL unintentionally.

<br/>

### Add Controller class file

Add an empty API Controller to the Examplium.Server/Controllers/ folder.

Name it "NotesController.cs".

Code:
```
using Examplium.Server.Services.Notes;
using Examplium.Shared.Models.Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Examplium.Server.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class NotesController : ControllerBase
    {
        private readonly INotesService _notesService;
        public NotesController(INotesService notesService)
        {
            _notesService = notesService;
        }

        [HttpPost("AddNote")]
        public async Task<ActionResult> AddNote(Note note)
        {
            var addNoteResponse = await _notesService.CreateNote(note);

            return Ok(addNoteResponse);
        }

        [HttpPost("UpdateNote")]
        public async Task<ActionResult> UpdateNote(Note note)
        {
            var updateNoteResponse = await _notesService.UpdateNote(note);

            return Ok(updateNoteResponse);
        }

        [HttpPost("DeleteNote")]
        public async Task<ActionResult> DeleteNote(Note note)
        {
            var updateNoteResponse = await _notesService.UpdateNote(note);

            return Ok(updateNoteResponse);
        }

        [HttpPost("GetNoteById")]
        public async Task<ActionResult> GetNoteById(int id)
        {
            var getNoteResponse = await _notesService.GetNoteById(id);

            return Ok(getNoteResponse);
        }
    }
}
```
