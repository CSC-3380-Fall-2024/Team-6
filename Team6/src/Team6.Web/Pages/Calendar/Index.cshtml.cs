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
    [Authorize]
    public class IndexModel : PageModel
    {
        private readonly CalendarRepository _repository;
        public Calendar<CalendarDay> Calendar { get; private set; } = new Calendar<CalendarDay>();
        public IEnumerable<CalendarEvent> Events { get; private set; } = new List<CalendarEvent>();
        
        [BindProperty]
        public CalendarEvent NewEvent { get; set; } = new() { 
            EventDate = DateTime.Now 
        };

        public IndexModel(CalendarRepository repository)
        {
            _repository = repository;
        }

        public async Task OnGetAsync()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim != null)
            {
                var userId = int.Parse(userIdClaim.Value);
                Events = await _repository.GetAllForUserAsync(userId);
                
                Console.WriteLine($"OnGetAsync - Loading events for user {userId}");
                Console.WriteLine($"Loaded events count: {Events.Count()}");
                foreach (var evt in Events)
                {
                    Console.WriteLine($"Event: {evt.Title} on {evt.EventDate:MM/dd/yyyy hh:mm tt}");
                }
            }
        }

        public async Task<IActionResult> OnPostAddEventAsync()
        {
            if (!ModelState.IsValid) 
            {
                await OnGetAsync();
                return Page();
            }
            
            try 
            {
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
                if (userIdClaim == null)
                {
                    return RedirectToPage("/Account/Login");
                }
                
                var userId = int.Parse(userIdClaim.Value);
                Console.WriteLine($"UserIdClaim: {userIdClaim.Value}");
                Console.WriteLine($"UserId: {userId}");
                Console.WriteLine($"Event Title: {NewEvent.Title}");
                Console.WriteLine($"Event Date: {NewEvent.EventDate}");

                await _repository.CreateAsync(NewEvent, userId);

                // Force reload of events after adding
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
                Console.WriteLine($"Error: {ex.Message}");
                Console.WriteLine($"Stack trace: {ex.StackTrace}");
                ModelState.AddModelError("", "Failed to create event");
                await OnGetAsync();
                return Page();
            }
        }

        public async Task<IActionResult> OnPostDeleteEventAsync(int id)
        {
            var userId = int.Parse(User.FindFirst("UserId")?.Value ?? "0");
            await _repository.DeleteAsync(id, userId);
            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostNavigateAsync(int direction)
        {
            var storedDate = TempData.Get<DateTime?>("CurrentDate");
            var currentDate = storedDate ?? DateTime.Today;
            currentDate = currentDate.AddMonths(direction);
            TempData.Set("CurrentDate", currentDate);
            Calendar.Navigate(currentDate - Calendar.NavigatedDate);
            await OnGetAsync();
            return Page();
        }
        
        public async Task<IActionResult> OnPostEditEventAsync(int id)
        {
            var userId = int.Parse(User.FindFirst("UserId")?.Value ?? "0");
            var eventToEdit = await _repository.GetByIdAsync(id, userId);
            if (eventToEdit == null) return NotFound();
            
            TempData["EditEventId"] = id;
            TempData["EditEventTitle"] = eventToEdit.Title;
            TempData["EditEventDate"] = eventToEdit.EventDate.ToString("yyyy-MM-ddTHH:mm");
            TempData["EditEventDescription"] = eventToEdit.Description;
            
            return RedirectToPage();
        }

        public override void OnPageHandlerExecuting(PageHandlerExecutingContext context)
        {
            var storedDate = TempData.Get<DateTime?>("CurrentDate");
            var currentDate = storedDate ?? DateTime.Today;
            Calendar.Navigate(currentDate - Calendar.NavigatedDate);
            base.OnPageHandlerExecuting(context);
        }
    }
}