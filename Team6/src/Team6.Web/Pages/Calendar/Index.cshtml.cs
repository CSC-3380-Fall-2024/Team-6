using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Filters;
using Team6.Core.Models;
using Team6.Data.Repositories;
using XCalendar.Core.Models;
using Microsoft.AspNetCore.Authorization;
using System.Text.Json;
using Team6.Web.Extensions;
using System.Security.Claims;

namespace Team6.Web.Pages.Calendar
{
    // ensure that the user has authorization to access the calendar page and then inherit the page model for function

    [Authorize]
    public class IndexModel : PageModel
    {
        private readonly CalendarRepository _repository; // calendar repository instance for operations
        public Calendar<CalendarDay> Calendar { get; private set; } = new Calendar<CalendarDay>(); // calendar instance for managing dates
        public IEnumerable<CalendarEvent> Events { get; private set; } = new List<CalendarEvent>(); // calendar instance for managing navigating the calendar
        

        // create new events 
        [BindProperty]
        public CalendarEvent NewEvent { get; set; } = new() { 
            EventDate = DateTime.Now 
        };

        // constructor to initialize the repository utilizing dependency injection so new instances don't have to be constantly created
        public IndexModel(CalendarRepository repository)
        {
            _repository = repository;
        }

        // handle page load 
        public async Task OnGetAsync()
        {
            // get the user claim from the authenticated session and load/display only their events
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim != null)
            {
                var userId = int.Parse(userIdClaim.Value);
                Events = await _repository.GetAllForUserAsync(userId);
                
                // debugging - can remove on final project
                Console.WriteLine($"OnGetAsync - Loading events for user {userId}");
                Console.WriteLine($"Loaded events count: {Events.Count()}");
                foreach (var evt in Events)
                {
                    Console.WriteLine($"Event: {evt.Title} on {evt.EventDate:MM/dd/yyyy hh:mm tt}");
                }
            }
        }

        // handle adding a new event via the modal form accessed from "Add new event" button
        public async Task<IActionResult> OnPostAddEventAsync()
        {
            if (!ModelState.IsValid) 
            {
                await OnGetAsync();
                return Page();
            }
            
            try 
            {
                // make sure user session is still authenticated
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
                if (userIdClaim == null)
                {
                    return RedirectToPage("/Account/Login");
                }
                // debugging and logging
                var userId = int.Parse(userIdClaim.Value);
                Console.WriteLine($"UserIdClaim: {userIdClaim.Value}");
                Console.WriteLine($"UserId: {userId}");
                Console.WriteLine($"Event Title: {NewEvent.Title}");
                Console.WriteLine($"Event Date: {NewEvent.EventDate}");

                // create the new event and then refresh the calendar to update it
                await _repository.CreateAsync(NewEvent, userId);
                Events = await _repository.GetAllForUserAsync(userId);
                Console.WriteLine($"Loaded events count: {Events.Count()}");
                foreach (var evt in Events)
                {
                    Console.WriteLine($"Event: {evt.Title} on {evt.EventDate:MM/dd/yyyy hh:mm tt}");
                }
                
                return RedirectToPage();
            }
            catch (Exception ex)
            {
                // debugging and error logging
                Console.WriteLine($"Error: {ex.Message}");
                Console.WriteLine($"Stack trace: {ex.StackTrace}");
                ModelState.AddModelError("", "Failed to create event");
                await OnGetAsync();
                return Page();
            }
        }

        // handle post requirement for event deletion
        public async Task<IActionResult> OnPostDeleteEventAsync(int id)
        {
            var claim = User.FindFirst("UserId"); // get the first userid claim for the current user
            var claimValue = (claim != null) ? claim.Value : null; // verify the claim exists, if not set it to null to avoid null reference error
            var userId = int.Parse(claimValue ?? "0"); // if the claim id doesn't exist, set it to a default of 0
            await _repository.DeleteAsync(id, userId); // delete the event only for the specified user
            return RedirectToPage(); // refresh calendar
        }

        // handles navigating between months where direction is + or - 1 based on the current month number
        public async Task<IActionResult> OnPostNavigateAsync(int direction)
        {
            // get the date that is currently displayed from temp data (temp data persists for one redirect)
            var storedDate = TempData.Get<DateTime?>("CurrentDate");
            var currentDate = storedDate ?? DateTime.Today; // default to today if there isn't a date stored
            currentDate = currentDate.AddMonths(direction);
            TempData.Set("CurrentDate", currentDate); // store new date in temp data
            Calendar.Navigate(currentDate - Calendar.NavigatedDate); // update the navigations state
            await OnGetAsync();
            return Page(); // refresh calendar
        }
        
        // handle event editing and storage in temp data
        public async Task<IActionResult> OnPostEditEventAsync(int id)
        {
            var claim = User.FindFirst("UserId"); // get the first userid claim for the current user
            var claimValue = (claim != null) ? claim.Value : null; // verify the claim exists, if not set it to null to avoid null reference error
            var userId = int.Parse(claimValue ?? "0"); // if the claim id doesn't exist, set it to a default of 0
            var eventToEdit = await _repository.GetByIdAsync(id, userId); 
            if (eventToEdit == null) return NotFound();
            
            // store event data for the edit form
            TempData["EditEventId"] = id;
            TempData["EditEventTitle"] = eventToEdit.Title;
            TempData["EditEventDate"] = eventToEdit.EventDate.ToString("yyyy-MM-ddTHH:mm");
            TempData["EditEventDescription"] = eventToEdit.Description;
            
            return RedirectToPage();
        }

        // ensure that the calendar is loaded/fully initialized before trying operations
        public override void OnPageHandlerExecuting(PageHandlerExecutingContext context)
        {
            var storedDate = TempData.Get<DateTime?>("CurrentDate");
            var currentDate = storedDate ?? DateTime.Today;
            Calendar.Navigate(currentDate - Calendar.NavigatedDate); // update calendar navigation state 
            base.OnPageHandlerExecuting(context); // call the implementation base to ensure that this executes before anything else
        }
    }
}