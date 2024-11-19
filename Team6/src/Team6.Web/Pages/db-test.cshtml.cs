using Microsoft.AspNetCore.Mvc.RazorPages;
using Team6.Core.Models;
using Team6.Data.Context;
using Team6.Data.Repositories;

namespace Team6.Web.Pages
{
    public class DbTestModel : PageModel
    {
        private readonly DatabaseContext _dbContext;
        private readonly NoteRepository _noteRepo;
        private readonly UserRepository _userRepo;

        public string TestResult { get; set; } = string.Empty;

        public DbTestModel(DatabaseContext dbContext, NoteRepository noteRepo, UserRepository userRepo)
        {
            _dbContext = dbContext;
            _noteRepo = noteRepo;
            _userRepo = userRepo;
        }

        public async Task OnGetAsync()
        {
            try
            {
                // Create test user
                var (success, user, errors) = await _userRepo.CreateUserAsync("testuser", "test@example.com", "TestPass123!");

                if (user != null)
                {
                    // Create note
                    var note = new Note 
                    { 
                        UserId = user.Id,
                        Title = "Test Note",
                        Content = "Test Content"
                    };
                    
                    var noteId = await _noteRepo.CreateNoteAsync(note);
                    
                    // Retrieve note
                    var retrievedNote = await _noteRepo.GetByIdAsync(noteId, user.Id);
                    
                    TestResult = $"Success! Note created and retrieved: {retrievedNote?.Title}";
                }
                else
                {
                    TestResult = $"User creation failed: {string.Join(", ", errors)}";
                }
            }
            catch (Exception ex)
            {
                TestResult = $"Error: {ex.Message}";
            }
        }
    }
}