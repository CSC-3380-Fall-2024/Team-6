using Dapper;
using Microsoft.AspNetCore.Identity;
using Team6.Core.Models;
using Team6.Data.Context;

namespace Team6.Data.Repositories
{
    public class UserRepository
    {
        private readonly DatabaseContext _context;
        private readonly IPasswordHasher<User> _passwordHasher;

        // initializes an instance of a UserRepository while calling the base constructor to establish the database connection
 
        public UserRepository(DatabaseContext context)
        {
            _context = context;
            _passwordHasher = new PasswordHasher<User>();
        }

        // retrieve user by username 
        public async Task<User?> GetByUsernameAsync(string username)
        {
            using var connection = _context.CreateConnection();
            return await connection.QueryFirstOrDefaultAsync<User>(
                "SELECT * FROM Users WHERE Username = @Username",
                new { Username = username }
            );
        }

        // creates a new user with the given credentials
        public async Task<(bool Success, User? User, string[] Errors)> CreateUserAsync(string username, string email, string password)
        {
            using var connection = _context.CreateConnection();
            try
            {
                // initialize a new user object 
                var user = new User
                {
                    Username = username,
                    Email = email,
                };

                // hash the password
                user.PasswordHash = _passwordHasher.HashPassword(user, password);

                // insert the new user into the database
                var userId = await connection.ExecuteScalarAsync<int>(@"
                    INSERT INTO Users (Username, Email, PasswordHash, CreatedAt)
                    VALUES (@Username, @Email, @PasswordHash, @CreatedAt);
                    SELECT last_insert_rowid();",
                    new { 
                        user.Username, 
                        user.Email, 
                        user.PasswordHash, 
                        user.CreatedAt 
                    });

                user.Id = userId;
                return (true, user, Array.Empty<string>());
            }
            catch (Exception)
            {
                return (false, null, new[] { "Username or email already exists" });
            }
        }

        // validate the login attempt
        public async Task<(bool Success, User? User, string[] Errors)> ValidateLoginAsync(string username, string password)
        {
            using var connection = _context.CreateConnection();
            
            // get user via username
            var user = await connection.QueryFirstOrDefaultAsync<User>(
                "SELECT * FROM Users WHERE Username = @Username",
                new { Username = username });

            if (user == null)
                return (false, null, new[] { "Invalid username or password" });

            
            // check to see if account is locked
            if (user.LockoutEnd.HasValue && user.LockoutEnd > DateTime.UtcNow)
                return (false, null, new[] { "Account is locked. Try again later." });

            
            // verify the hashed passwords match
            var result = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, password);


            // if the password verification fails after 5 attempts, lock the account
            if (result == PasswordVerificationResult.Failed)
            {
                user.FailedLoginAttempts++;
                if (user.FailedLoginAttempts >= 5)
                {
                    user.LockoutEnd = DateTime.UtcNow.AddMinutes(15);
                }
                
                // update the number of failed attempts in the database
                await connection.ExecuteAsync(@"
                    UPDATE Users 
                    SET FailedLoginAttempts = @FailedLoginAttempts, 
                        LockoutEnd = @LockoutEnd 
                    WHERE Id = @Id",
                    user);
                    
                return (false, null, new[] { "Invalid username or password" });
            }

            // reset failed attempts and update last login
            await connection.ExecuteAsync(@"
                UPDATE Users 
                SET LastLogin = @Now, 
                    FailedLoginAttempts = 0, 
                    LockoutEnd = NULL 
                WHERE Id = @Id",
                new { Now = DateTime.UtcNow, user.Id });

            return (true, user, Array.Empty<string>());
        }

        // reset the user password
        public async Task<bool> ResetPasswordAsync(string email, string username, string newPassword)
        {
            using var connection = _context.CreateConnection();

            // verify that the username and email combination exists in the database
            var user = await connection.QueryFirstOrDefaultAsync<User>(
                "SELECT * FROM Users WHERE Email = @Email AND Username = @Username",
                new { Email = email, Username = username });

            if (user == null) 
            {
                return false;
            }

            user.PasswordHash = _passwordHasher.HashPassword(user, newPassword);

            // update all the password and login data for the user
            await connection.ExecuteAsync(@"
                UPDATE Users
                SET PasswordHash = @PasswordHash,
                    LastPasswordChange = @Now,
                    FailedLoginAttempts = 0,
                    LockoutEnd = NULL
                WHERE Id = @Id",
                new { user.PasswordHash, Now = DateTime.UtcNow, user.Id });
            
            return true;
            
        }
    }
}