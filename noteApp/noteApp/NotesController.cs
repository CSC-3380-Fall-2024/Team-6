namespace noteApp.Data;
using Microsoft.AspNetCore.Mvc;

[Route("api/[controller]")]
[ApiController]
public class NotesController : ControllerBase
{
    private readonly NoteRepository _noteRepository;

    public NotesController(NoteRepository noteRepository)
    {
        _noteRepository = noteRepository;
    }

    [HttpGet]
    public async Task<IEnumerable<Note>> GetNotes()
    {
        return await _noteRepository.GetAllNotesAsync();
    }

    [HttpPost]
    public async Task<IActionResult> AddNote([FromBody] Note note)
    {
        await _noteRepository.AddNoteAsync(note);
        return Ok(note);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateNote(int id, [FromBody] Note note)
    {
        if (note == null || id != note.Id || string.IsNullOrWhiteSpace(note.Title) || string.IsNullOrWhiteSpace(note.Content))
        {
            return BadRequest("Invalid note data.");
        }

        await _noteRepository.UpdateNoteAsync(note.Id, note.Title, note.Content);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteNote(int id)
    {
        await _noteRepository.DeleteNoteAsync(id);
        return NoContent();
    }
}

