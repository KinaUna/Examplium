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
            var updateNoteResponse = await _notesService.DeleteNote(note);

            return Ok(updateNoteResponse);
        }

        [HttpPost("GetNoteById")]
        public async Task<ActionResult> GetNoteById(int id)
        {
            var getNoteResponse = await _notesService.GetNoteById(id);

            return Ok(getNoteResponse);
        }

        [HttpGet("GetMyNotes")]
        public async Task<ActionResult> GetMyNotes()
        {
            var getMyNotesResponse = await _notesService.GetMyNotes();

            return Ok(getMyNotesResponse);
        }
    }
}
