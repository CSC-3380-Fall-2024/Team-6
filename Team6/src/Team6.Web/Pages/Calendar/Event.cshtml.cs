using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Team6.Core.Models;
using Team6.Data.Repositories;
using System.Security.Claims;

namespace Team6.Web.Pages.Calendar
{
    public class EventModel : PageModel
    {
        private readonly CalendarRepository _repository; // calendar repository instance for operations

        // handle the calendar event once it is being viewed and/or edited in the dedicated modal 
        [BindProperty]
        public CalendarEvent Event { get; set; } = null!;
        public bool IsEditing { get; set; } // determine if the event is in edit mode to control which ui is shown

        // constructor to initialize the repository utilizing dependency injection so new instances don't have to be constantly created
        public EventModel(CalendarRepository repository)
        {
            _repository = repository;
        }

        // handle the get request to get to the dedicated view of the calendar event
        public async Task<IActionResult> OnGetAsync(int id, bool edit = false)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier); // authenticate user 
            if (userIdClaim == null)
                return RedirectToPage("/Account/Login"); 

            var userId = int.Parse(userIdClaim.Value);
            Event = await _repository.GetByIdAsync(id, userId); // use the repository to get all of the event details from the database

            if (Event == null)
                return NotFound();

            IsEditing = edit; 
            return Page();
        }

        // handle post requests for updating events 
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
                return Page();

            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier); // verify user is still authenticated
            if (userIdClaim == null)
                return RedirectToPage("/Account/Login");

            var userId = int.Parse(userIdClaim.Value);
            await _repository.UpdateAsync(Event, userId); // use repository to update the event in the database
            
            return RedirectToPage("./Event", new { id = Event.Id });
        }
        
        // handle post request for deleting an event 
        public async Task<IActionResult> OnPostDeleteAsync(int id)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier); // authenticate user
            if (userIdClaim == null)
                return RedirectToPage("/Account/Login");

            var userId = int.Parse(userIdClaim.Value);
            await _repository.DeleteAsync(id, userId); // use repository to delete event from database
            
            return RedirectToPage("./Index");
        }
    }
}