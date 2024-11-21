using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
using System.ComponentModel.DataAnnotations;
using Team6.Data.Repositories;

namespace Team6.Web.Pages.Account
{
    public class LoginModel : PageModel
    {
        private readonly UserRepository _userRepository;
        private readonly ILogger<LoginModel> _logger;

        [BindProperty]
        [Required(ErrorMessage = "Username is required")]
        public string Username { get; set; } = string.Empty;

        [BindProperty]
        [Required(ErrorMessage = "Password is required")]
        [DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;

        public string ErrorMessage { get; set; } = string.Empty;

        public LoginModel(UserRepository userRepository, ILogger<LoginModel> logger)
        {
            _userRepository = userRepository;
            _logger = logger;
        }

        public void OnGet()
        {
            _logger.LogInformation("Login page accessed");
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var (success, user, errors) = await _userRepository.ValidateLoginAsync(Username, Password);
            if (!success)
            {
                ErrorMessage = string.Join(", ", errors);
                _logger.LogWarning("Login failed for user {Username}", Username);
                return Page();
            }

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
            };

            var identity = new ClaimsIdentity(claims, "Cookies");
            var principal = new ClaimsPrincipal(identity);

            await HttpContext.SignInAsync("Cookies", principal);
            _logger.LogInformation("User {Username} logged in successfully", Username);

            return RedirectToPage("/Calendar/Index");
        }
    }
}