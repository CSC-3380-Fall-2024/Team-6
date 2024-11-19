using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Team6.Core.Models;
using Team6.Data.Repositories;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using System.ComponentModel.DataAnnotations;

namespace Team6.Web.Pages.Reflections
{
    [Authorize]
    public class UploadModel : PageModel
    {
        private readonly ReflectionRepository _reflectionRepository;
        private readonly ReflectionDocumentRepository _documentRepository;
        private readonly ILogger<UploadModel> _logger;

        [BindProperty]
        [Required(ErrorMessage = "Title is required")]
        public string Title { get; set; } = string.Empty;

        [BindProperty]
        [Required(ErrorMessage = "Please select a PDF file")]
        public IFormFile PdfFile { get; set; }

        public string ErrorMessage { get; set; } = string.Empty;

        public UploadModel(
            ReflectionRepository reflectionRepository,
            ReflectionDocumentRepository documentRepository,
            ILogger<UploadModel> logger)
        {
            _reflectionRepository = reflectionRepository;
            _documentRepository = documentRepository;
            _logger = logger;
        }

        private int GetUserId() => int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value!);

        public async Task<IActionResult> OnPostAsync()
        {
            try
            {
                _logger.LogInformation("Starting file upload process");

                if (!ModelState.IsValid)
                {
                    _logger.LogWarning("Model state is invalid");
                    return Page();
                }

                if (PdfFile.Length > 10 * 1024 * 1024)
                {
                    ErrorMessage = "File size must be less than 10MB";
                    _logger.LogWarning("File too large: {Size} bytes", PdfFile.Length);
                    return Page();
                }

                _logger.LogInformation("File type: {ContentType}", PdfFile.ContentType);
                if (PdfFile.ContentType != "application/pdf")
                {
                    ErrorMessage = "Only PDF files are allowed";
                    return Page();
                }

                var userId = GetUserId();
                _logger.LogInformation("Processing upload for user {UserId}", userId);

                // Read PDF file
                using var memoryStream = new MemoryStream();
                await PdfFile.CopyToAsync(memoryStream);
                var pdfContent = memoryStream.ToArray();
                _logger.LogInformation("File read successfully, size: {Size} bytes", pdfContent.Length);

                // Create document
                var document = new ReflectionDocument
                {
                    Title = Title,
                    FileType = "application/pdf",
                    FileSize = (int)PdfFile.Length,
                    FilePath = PdfFile.FileName,
                    PdfContent = pdfContent
                };

                _logger.LogInformation("Creating document record");
                var documentId = await _documentRepository.CreateAsync(document, userId);
                _logger.LogInformation("Document created with ID: {DocumentId}", documentId);

                // Create reflection
                var reflection = new Reflection
                {
                    Title = Title,
                    DocumentId = documentId
                };

                _logger.LogInformation("Creating reflection record");
                await _reflectionRepository.CreateAsync(reflection, userId);
                _logger.LogInformation("Reflection created successfully");

                return RedirectToPage("./Index");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error details: {Message}", ex.Message);
                ErrorMessage = $"Error uploading file: {ex.Message}";
                return Page();
            }
        }   
    }
}