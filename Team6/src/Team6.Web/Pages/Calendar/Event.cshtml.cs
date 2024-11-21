using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Team6.Core.Models;
using Team6.Data.Repositories;
using System.Security.Claims;

namespace Team6.Web.Pages.Calendar
{
    public class EventModel : PageModel
    {
        private readonly CalendarRepository _repository;

        [BindProperty]
        public CalendarEvent Event { get; set; } = null!;
        public bool IsEditing { get; set; }

        public EventModel(CalendarRepository repository)
        {
            _repository = repository;
        }

        public async Task<IActionResult> OnGetAsync(int id, bool edit = false)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
                return RedirectToPage("/Account/Login");

            var userId = int.Parse(userIdClaim.Value);
            Event = await _repository.GetByIdAsync(id, userId);

            if (Event == null)
                return NotFound();

            IsEditing = edit;
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
                return Page();

            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
                return RedirectToPage("/Account/Login");

            var userId = int.Parse(userIdClaim.Value);
            await _repository.UpdateAsync(Event, userId);
            
            return RedirectToPage("./Event", new { id = Event.Id });
        }

        public async Task<IActionResult> OnPostDeleteAsync(int id)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
                return RedirectToPage("/Account/Login");

            var userId = int.Parse(userIdClaim.Value);
            await _repository.DeleteAsync(id, userId);
            
            return RedirectToPage("./Index");
        }
    }
}