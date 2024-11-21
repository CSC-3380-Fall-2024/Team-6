using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using Team6.Data.Repositories;

namespace Team6.Web.Pages.Account
{

    // inherit the page model for registration function
    public class RegisterModel : PageModel
    {
        private readonly UserRepository _userRepository; // repo for user database operations
        private readonly ILogger<RegisterModel> _logger; // logger for debugging

        // username input field with string length requirements
        [BindProperty]
        [Required(ErrorMessage = "Username is required")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Username must be between 3 and 50 characters")]
        public string Username { get; set; } = string.Empty;

        // email input field -> there isn't any actual verifcation in place yet
        [BindProperty]
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email address")]
        public string Email { get; set; } = string.Empty;

        // password input field with length requirements
        [BindProperty]
        [Required(ErrorMessage = "Password is required")]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "Password must be at least 6 characters")]
        [DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;

        public string ErrorMessage { get; set; } = string.Empty;
        
        // constructor to initialize the repository and logger utilizing dependency injection so new instances don't have to be constantly created
        public RegisterModel(UserRepository userRepository, ILogger<RegisterModel> logger)
        {
            _userRepository = userRepository;
            _logger = logger;
        }

        // handle the get requests to access the registration page and outputs that to the terminal
        public void OnGet()
        {
            _logger.LogInformation("Register page accessed");
        }

        // handle the post requests for user registration attempts
        public async Task<IActionResult> OnPostAsync()
        {
            // make sure that all the input forms meet the requirements (length, complexity, etc) 
            if (!ModelState.IsValid)
            {
                return Page();
            }

            // use the user repository to validate the login and return info for each field
            var (success, user, errors) = await _userRepository.CreateUserAsync(Username, Email, Password);
            if (!success)
            {
                ErrorMessage = string.Join(", ", errors);
                _logger.LogWarning("Registration failed for username {Username}", Username);
                return Page(); // if failure re-route to registation again
            }

            _logger.LogInformation("User {Username} registered successfully", Username);
            return RedirectToPage("/Account/Login");
        }
    }
}