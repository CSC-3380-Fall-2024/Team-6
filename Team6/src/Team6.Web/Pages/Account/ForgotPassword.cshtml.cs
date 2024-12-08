using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using Team6.Data.Repositories;

namespace Team6.Web.Pages.Account
{
    public class ForgotPasswordModel : PageModel
    {
        private readonly UserRepository _userRepository;
        private readonly ILogger<ForgotPasswordModel> _logger;

        [BindProperty, Required, EmailAddress]
        public string Email { get; set; } = string.Empty;

        [BindProperty, Required]
        public string Username { get; set; } = string.Empty;

        [BindProperty, Required]
        [StringLength(100, MinimumLength = 6)]
        public string NewPassword { get; set; } = string.Empty;

        public string? ErrorMessage { get; set; }

        public ForgotPasswordModel(UserRepository userRepository, ILogger<ForgotPasswordModel> logger)
        {
            _userRepository = userRepository;
            _logger = logger;
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid) return Page();

            var success = await _userRepository.ResetPasswordAsync(Email, Username, NewPassword);
            if (success)
            {
                _logger.LogInformation("Password reset successful for user {Username}", Username);
                return RedirectToPage("./Login");
            }

            ErrorMessage = "Unable to reset password. Please verify your email and username.";
            return Page();
        }
    }
}