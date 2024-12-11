using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using Team6.Data.Repositories;

namespace Team6.Web.Pages.Account
{
   // inherit the page model for password reset functionality
   public class ForgotPasswordModel : PageModel
   {
       private readonly UserRepository _userRepository; // user repository instance for operations
       private readonly ILogger<ForgotPasswordModel> _logger; // logger instance for tracking activity

       // bind and validate email input field with required format
       [BindProperty, Required, EmailAddress]
       public string Email { get; set; } = string.Empty;

       // bind and require username input field 
       [BindProperty, Required]
       public string Username { get; set; } = string.Empty;

       // bind and validate new password with length requirements
       [BindProperty, Required]
       [StringLength(100, MinimumLength = 1)]
       public string NewPassword { get; set; } = string.Empty;

       public string? ErrorMessage { get; set; } // store error message to display to user if reset fails

       // constructor to initialize the repository and logger utilizing dependency injection so new instances don't have to be constantly created
       public ForgotPasswordModel(UserRepository userRepository, ILogger<ForgotPasswordModel> logger)
       {
           _userRepository = userRepository;
           _logger = logger;
       }

       // handle post requests for password reset attempts
       public async Task<IActionResult> OnPostAsync()
       {
           if (!ModelState.IsValid) return Page(); // verify input meets requirements

           // use repository to attempt password reset and store result
           var success = await _userRepository.ResetPasswordAsync(Email, Username, NewPassword);
           
           if (success)
           {
               _logger.LogInformation("Password reset successful for user {Username}", Username); // log successful reset
               return RedirectToPage("./Login"); // return to login page
           }

           ErrorMessage = "Unable to reset password. Please verify your email and username."; // set error message for failed attempt
           return Page(); // stay on reset page
       }
   }
}