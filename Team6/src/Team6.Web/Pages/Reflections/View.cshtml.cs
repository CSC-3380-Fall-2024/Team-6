
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Team6.Core.Models;
using Team6.Data.Repositories;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace Team6.Web.Pages.Reflections
{
    [Authorize]
    public class ViewModel : PageModel
    {
        private readonly ReflectionRepository _reflectionRepository;
        private readonly ReflectionDocumentRepository _documentRepository;
        
        public Reflection? Reflection { get; set; }
        public string? PdfDataUrl { get; set; }

        public ViewModel(ReflectionRepository reflectionRepository, ReflectionDocumentRepository documentRepository)
        {
            _reflectionRepository = reflectionRepository;
            _documentRepository = documentRepository;
        }

        private int GetUserId() => int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value!);

        public async Task<IActionResult> OnGetAsync(int id)
        {
            var userId = GetUserId();
            Reflection = await _reflectionRepository.GetByIdAsync(id, userId);
            
            if (Reflection == null || Reflection.DocumentId == null)
            {
                return NotFound();
            }

            var document = await _documentRepository.GetByIdAsync(Reflection.DocumentId.Value, userId);
            if (document?.PdfContent != null)
            {
                PdfDataUrl = $"data:application/pdf;base64,{Convert.ToBase64String(document.PdfContent)}";
            }

            return Page();
        }
    }
}