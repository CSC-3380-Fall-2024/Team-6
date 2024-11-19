using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using Team6.Data.Repositories;

namespace Team6.Web.Pages.Account
{
    public class RegisterModel : PageModel
    {
        private readonly UserRepository _userRepository;
        private readonly ILogger<RegisterModel> _logger;

        [BindProperty]
        [Required(ErrorMessage = "Username is required")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Username must be between 3 and 50 characters")]
        public string Username { get; set; } = string.Empty;

        [BindProperty]
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email address")]
        public string Email { get; set; } = string.Empty;

        [BindProperty]
        [Required(ErrorMessage = "Password is required")]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "Password must be at least 6 characters")]
        [DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;

        public string ErrorMessage { get; set; } = string.Empty;

        public RegisterModel(UserRepository userRepository, ILogger<RegisterModel> logger)
        {
            _userRepository = userRepository;
            _logger = logger;
        }

        public void OnGet()
        {
            _logger.LogInformation("Register page accessed");
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var (success, user, errors) = await _userRepository.CreateUserAsync(Username, Email, Password);
            if (!success)
            {
                ErrorMessage = string.Join(", ", errors);
                _logger.LogWarning("Registration failed for username {Username}", Username);
                return Page();
            }

            _logger.LogInformation("User {Username} registered successfully", Username);
            return RedirectToPage("/Account/Login");
        }
    }
}