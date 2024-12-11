
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Team6.Core.Models;
using Team6.Data.Repositories;  
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace Team6.Web.Pages.Reflections
{
   [Authorize] // require authentication to view reflections
   public class ViewModel : PageModel
   {
       private readonly ReflectionRepository _reflectionRepository; // repository instance for reflection operations
       private readonly ReflectionDocumentRepository _documentRepository; // repository instance for document operations 

       public Reflection? Reflection { get; set; } // store reflection details to display
       public string? PdfDataUrl { get; set; } // store PDF data as base64 string for viewing

       // constructor to initialize repositories utilizing dependency injection so new instances don't have to be constantly created
       public ViewModel(ReflectionRepository reflectionRepository, ReflectionDocumentRepository documentRepository)
       {
           _reflectionRepository = reflectionRepository;
           _documentRepository = documentRepository;
       }

       // helper method to get current user's ID from claims
       private int GetUserId() => int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value!);

       // handle get requests to view a specific reflection
       public async Task<IActionResult> OnGetAsync(int id)
       {
           var userId = GetUserId(); // get authenticated user ID
           Reflection = await _reflectionRepository.GetByIdAsync(id, userId); // use repository to get reflection details

           // verify reflection exists and has an associated document
           if (Reflection == null || Reflection.DocumentId == null)
           {
               return NotFound();
           }

           // get associated document from repository
           var document = await _documentRepository.GetByIdAsync(Reflection.DocumentId.Value, userId);

           // if document has content, convert to base64 for display
           if (document?.PdfContent != null)
           {
               PdfDataUrl = $"data:application/pdf;base64,{Convert.ToBase64String(document.PdfContent)}";
           }

           return Page(); // display the reflection view page
       }
   }
}