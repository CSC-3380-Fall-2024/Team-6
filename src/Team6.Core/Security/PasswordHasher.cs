using System.Security.Cryptography;

namespace Team6.Core.Security
{
    // handle the password hashing and password verification using .NET's PasswordHasher
    public class PasswordHasher
    {

        // create a private instance of the password hasher 
        private readonly PasswordHasher<User> _hasher;

        // create minimum password strength requirements
        private const int MinPasswordLength = 8;
        private const int MinNumbers = 1;
        private const int MinSpecialChars = 1;
        private const int MinUpperCaseChars = 1;

        // constructor to initialize the hasher
        public PasswordHasher()
        {
            _hasher = new PasswordHasher<User>();
        }

        // validate the password's strengtha against requirements
        public (bool IsValid, string[] Errors) ValidatePassword(string password)
        {
            // create a list of errors 
            var errors = new List<string>();

            // check min length
            if (password.Length < MinPasswordLength)
            {
                errors.Add($"Password must be at least {MinPasswordLength} characters.");
            }

            // check min numbers
            if (password.Count(char.IsDigit) < MinNumbers)
            {
                errors.Add($"Password must contain at least {MinNumbers} numbers.");
            }

            // check min special characters
            if (password.Count(char => !char.IsLetterOrDigit(c)) < MinSpecialChars)
            {
                errors.Add($"Password must contain at least {MinSpecialChars} special characters.");
            }

            // check min upper case letters
            if (password.Count(char.IsUpper) < MinUpperCaseChars) 
            {
                errors.Add($"Password must contain at least {MinUpperCaseChars} upper case letters.");
            }

            return (errors.Count == 0, errors.ToArray());
        }

        // hash the password using .NET's implementation
        public (bool Success, string Result, string[] Errors)  HashPassword(string password)
        {   
            // validate password 
            var (IsValid, errors) = ValidatePassword(password);
            if (!IsValid)
            {
                return (false, string.Empty, errors);
            }
            // create a temporary user instance
            var user  = new User();
            // hash using PBKDF2 with sha-256
            var hashedPassword =  _hasher.HashPassword(user, password);
            
            return (true, hashedPassword, Array.Empty<string>());
        }

        // verify a password against the stored hash
        public (bool Success, bool NeedsRehash, string NewHash) VerifyPassword(string hashedPassword, string inputPassword)
        {   
            var user = new User();

            // verify the password, returning PasswordVerificationResult.Success if there is a match
            var result = _hasher.VerifyHashedPassword(
                user,
                hashedPassword,
                inputPassword
            );

            if (result == PasswordVerifcationResult.Failed)
            {
                return (false, false, string.Empty);
            }

            // rehash the password if needed
            if (result == PasswordVerificationResult.SuccessRehashNeeded)
            {
                var newHash = _hasher.HashPassword(user, inputPassword);
                return (true, true, newHash);
            }
            
            return (true, false, string.Empty);
        }
    }
}