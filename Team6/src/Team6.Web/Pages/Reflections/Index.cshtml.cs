using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Team6.Core.Models;
using Team6.Data.Repositories;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace Team6.Web.Pages.Reflections
{
    [Authorize] // require authentication to access reflection pages
    public class IndexModel : PageModel
    {
        private readonly ReflectionRepository _reflectionRepository; // reflection repository instance for operations 
        private readonly ILogger<IndexModel> _logger; // logger instance for tracking activity

        public IEnumerable<Reflection> Reflections { get; set; } = Enumerable.Empty<Reflection>(); // store list of reflections to display

        // constructor to initialize the repository and logger utilizing dependency injection so new instances don't have to be constantly created
        public IndexModel(ReflectionRepository reflectionRepository, ILogger<IndexModel> logger)
        {
            _reflectionRepository = reflectionRepository;
            _logger = logger;
        }

        // helper method to get current user's ID from claims
        private int GetUserId() => int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value!);

        // handle get requests to view reflection index page
        public async Task OnGetAsync()
        {
            var userId = GetUserId(); // get authenticated user ID
            Reflections = await _reflectionRepository.GetAllForUserAsync(userId); // use repository to get user's reflections from database
        }

        // handle post requests for deleting a reflection
        public async Task<IActionResult> OnPostDeleteAsync(int id)
        {
            var userId = GetUserId(); // verify user is still authenticated
            await _reflectionRepository.DeleteAsync(id, userId); // use repository to delete reflection from database
            return RedirectToPage(); // reload the page
        }
    }
}