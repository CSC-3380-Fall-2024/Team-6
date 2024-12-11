using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Team6.Core.Models;
using Team6.Data.Repositories;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using System.ComponentModel.DataAnnotations;

namespace Team6.Web.Pages.Reflections
{
   [Authorize] // require authentication to access upload functionality
   public class UploadModel : PageModel
   {
       private readonly ReflectionRepository _reflectionRepository; // repository instance for reflection operations
       private readonly ReflectionDocumentRepository _documentRepository; // repository instance for document operations
       private readonly ILogger<UploadModel> _logger; // logger instance for tracking upload activity

       // bind and require title input for the reflection
       [BindProperty]
       [Required(ErrorMessage = "Title is required")]
       public string Title { get; set; } = string.Empty;

       // bind and require PDF file upload
       [BindProperty]
       [Required(ErrorMessage = "Please select a PDF file")]
       public IFormFile PdfFile { get; set; }

       public string ErrorMessage { get; set; } = string.Empty; // store error messages to display to user

       // constructor to initialize repositories and logger utilizing dependency injection so new instances don't have to be constantly created
       public UploadModel(
           ReflectionRepository reflectionRepository,
           ReflectionDocumentRepository documentRepository,
           ILogger<UploadModel> logger)
       {
           _reflectionRepository = reflectionRepository;
           _documentRepository = documentRepository;
           _logger = logger;
       }

       // helper method to get current user's ID from claims
       private int GetUserId() => int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value!);

       // handle post requests for uploading new reflections
       public async Task<IActionResult> OnPostAsync()
       {
           try
           {
               _logger.LogInformation("Starting file upload process"); // log start of upload

               if (!ModelState.IsValid)
               {
                   _logger.LogWarning("Model state is invalid"); // log validation failure
                   return Page();
               }

               // verify file size is under 10MB limit
               if (PdfFile.Length > 10 * 1024 * 1024)
               {
                   ErrorMessage = "File size must be less than 10MB";
                   _logger.LogWarning("File too large: {Size} bytes", PdfFile.Length);
                   return Page();
               }

               // verify file is PDF type
               _logger.LogInformation("File type: {ContentType}", PdfFile.ContentType);
               if (PdfFile.ContentType != "application/pdf")
               {
                   ErrorMessage = "Only PDF files are allowed";
                   return Page();
               }

               var userId = GetUserId(); // get authenticated user ID
               _logger.LogInformation("Processing upload for user {UserId}", userId);

               // read PDF file into memory
               using var memoryStream = new MemoryStream();
               await PdfFile.CopyToAsync(memoryStream);
               var pdfContent = memoryStream.ToArray();
               _logger.LogInformation("File read successfully, size: {Size} bytes", pdfContent.Length);

               // create document record with file details
               var document = new ReflectionDocument
               {
                   Title = Title,
                   FileType = "application/pdf",
                   FileSize = (int)PdfFile.Length,
                   FilePath = PdfFile.FileName,
                   PdfContent = pdfContent
               };

               // save document to database using repository
               _logger.LogInformation("Creating document record");
               var documentId = await _documentRepository.CreateAsync(document, userId);
               _logger.LogInformation("Document created with ID: {DocumentId}", documentId);

               // create reflection record linked to document
               var reflection = new Reflection
               {
                   Title = Title,
                   DocumentId = documentId
               };

               // save reflection to database using repository
               _logger.LogInformation("Creating reflection record");
               await _reflectionRepository.CreateAsync(reflection, userId);
               _logger.LogInformation("Reflection created successfully");

               return RedirectToPage("./Index"); // return to reflections list
           }
           catch (Exception ex)
           {
               // log any errors and display to user
               _logger.LogError(ex, "Error details: {Message}", ex.Message);
               ErrorMessage = $"Error uploading file: {ex.Message}";
               return Page();
           }
       }   
   }
}