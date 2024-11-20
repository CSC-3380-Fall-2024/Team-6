using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Team6.Core.Models;
using Team6.Data.Repositories;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Team6.Web.Pages.Notes
{
    public class NotesModel : PageModel
    {
        private readonly NoteRepository _noteRepository;

        // Constructor with dependency injection for the NoteRepository
        public NotesModel(NoteRepository noteRepository)
        {
            _noteRepository = noteRepository;
        }

        [BindProperty]
        public Note NewNote { get; set; } = new Note();  // For new note form

        public List<Note> Notes { get; set; } = new List<Note>();  // List to hold existing notes

        // Get the notes for the current user
        public async Task OnGetAsync()
        {
            // Fetch the user ID from the current authenticated user
            var userId = GetUserId();
            await LoadNotes(userId);
        }

        // Save a new note
        public async Task<IActionResult> OnPostSaveAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();  // If model is invalid, stay on the page
            }

            var note = new Note
            {
                Title = NewNote.Title,
                Content = NewNote.Content,
                UserId = GetUserId(), // Assuming UserId should be assigned when creating the note
                CreatedAt = DateTime.Now // Optionally, add created timestamp
            };

            // Create the note in the database
            await _noteRepository.CreateNoteAsync(note);

            // Reset NewNote for the next entry
            NewNote = new Note();

            // Reload the list of notes
            var userId = GetUserId();
            await LoadNotes(userId);

            return RedirectToPage();  // Redirect to refresh the page
        }

        // Edit an existing note
        public async Task<IActionResult> OnPostEditAsync(int id)
        {
            var userId = GetUserId();  // Get the userId for the logged-in user

            // Fetch the note by id and userId
            var note = await _noteRepository.GetByIdAsync(id, userId);

            if (note != null)
            {
                NewNote = new Note
                {
                    Id = note.Id,
                    Title = note.Title,
                    Content = note.Content
                };
            }

            // Reload the notes after editing
            await LoadNotes(userId);
            return Page();
        }

        // Delete a note by its ID
        public async Task<IActionResult> OnPostDeleteAsync(int id)
        {
            var userId = GetUserId(); // Get the user ID from the session
            await _noteRepository.DeleteNoteAsync(id, userId); // Delete the note
            await LoadNotes(userId); // Reload the notes after deletion
            return RedirectToPage();  // Redirect to the same page
        }

        // Load the list of notes for a specific user
        private async Task LoadNotes(int userId)
        {
            var notesFromRepo = await _noteRepository.GetUserNotesAsync(userId);
            Notes = notesFromRepo.ToList();  // Assign the fetched notes to the Notes property
        }

        // Helper method to get the user ID from the current authenticated user
        private int GetUserId()
        {
            // Assuming the user is authenticated and has a claim named "UserId"
            var userIdString = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            return userIdString != null ? int.Parse(userIdString) : 0;
        }
    }
}

