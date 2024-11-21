// imports for web authentication 
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
using System.ComponentModel.DataAnnotations;
using Team6.Data.Repositories;

namespace Team6.Web.Pages.Account
{

    // inherit the page model for handling login functions 
    public class LoginModel : PageModel
    {
        private readonly UserRepository _userRepository; // repo for user database operations
        private readonly ILogger<LoginModel> _logger; // logger for debugging

        // username property with validation
        [BindProperty]
        [Required(ErrorMessage = "Username is required")]
        public string Username { get; set; } = string.Empty;
        
        // password property with validation 
        [BindProperty]
        [Required(ErrorMessage = "Password is required")]
        [DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;

        public string ErrorMessage { get; set; } = string.Empty;

        // constructor to initialize the repository and logger utilizing dependency injection so new instances don't have to be constantly created
        public LoginModel(UserRepository userRepository, ILogger<LoginModel> logger)
        {
            _userRepository = userRepository;
            _logger = logger;
        }

        // handle the get requests to access the login page and outputs that to the terminal
        public void OnGet()
        {
            _logger.LogInformation("Login page accessed");
        }

        // handle the post rrequests for login attempts
        public async Task<IActionResult> OnPostAsync()
        {
            // make sure that all the input forms meet the requirements (length, complexity, etc) 
            if (!ModelState.IsValid)
            {
                return Page();
            }

            // use the user repository to validate the login and return info for each field
            var (success, user, errors) = await _userRepository.ValidateLoginAsync(Username, Password);
            // if validation fails, re-route to login
            if (!success)
            {
                ErrorMessage = string.Join(", ", errors);
                _logger.LogWarning("Login failed for user {Username}", Username);
                return Page();
            }

            // create a claims for a user's session 
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
            };

            var identity = new ClaimsIdentity(claims, "Cookies"); // use cookies as the authentication type for the user's session
            var principal = new ClaimsPrincipal(identity); // ASP.NET core's basic way to track the authenticated user

            await HttpContext.SignInAsync("Cookies", principal); // sign the user in via cookies to maintain session
            _logger.LogInformation("User {Username} logged in successfully", Username);

            return RedirectToPage("/Calendar/Index");
        }
    }
}