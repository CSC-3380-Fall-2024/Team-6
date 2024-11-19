using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Team6.Core.Models;
using Team6.Data.Repositories;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace Team6.Web.Pages.Reflections
{
    [Authorize]
    public class IndexModel : PageModel
    {
        private readonly ReflectionRepository _reflectionRepository;
        private readonly ILogger<IndexModel> _logger;

        public IEnumerable<Reflection> Reflections { get; set; } = Enumerable.Empty<Reflection>();

        public IndexModel(ReflectionRepository reflectionRepository, ILogger<IndexModel> logger)
        {
            _reflectionRepository = reflectionRepository;
            _logger = logger;
        }

        private int GetUserId() => int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value!);

        public async Task OnGetAsync()
        {
            var userId = GetUserId();
            Reflections = await _reflectionRepository.GetAllForUserAsync(userId);
        }

        public async Task<IActionResult> OnPostDeleteAsync(int id)
        {
            var userId = GetUserId();
            await _reflectionRepository.DeleteAsync(id, userId);
            return RedirectToPage();
        }
    }
}