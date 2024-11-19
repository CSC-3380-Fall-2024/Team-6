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

        public UserRepository(DatabaseContext context)
        {
            _context = context;
            _passwordHasher = new PasswordHasher<User>();
        }

        public async Task<User?> GetByUsernameAsync(string username)
        {
            using var connection = _context.CreateConnection();
            return await connection.QueryFirstOrDefaultAsync<User>(
                "SELECT * FROM Users WHERE Username = @Username",
                new { Username = username }
            );
        }

        public async Task<(bool Success, User? User, string[] Errors)> CreateUserAsync(string username, string email, string password)
        {
            using var connection = _context.CreateConnection();
            try
            {
                var user = new User
                {
                    Username = username,
                    Email = email,
                };

                // Hash the password
                user.PasswordHash = _passwordHasher.HashPassword(user, password);

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

        public async Task<(bool Success, User? User, string[] Errors)> ValidateLoginAsync(string username, string password)
        {
            using var connection = _context.CreateConnection();
            var user = await connection.QueryFirstOrDefaultAsync<User>(
                "SELECT * FROM Users WHERE Username = @Username",
                new { Username = username });

            if (user == null)
                return (false, null, new[] { "Invalid username or password" });

            if (user.LockoutEnd.HasValue && user.LockoutEnd > DateTime.UtcNow)
                return (false, null, new[] { "Account is locked. Try again later." });

            var result = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, password);

            if (result == PasswordVerificationResult.Failed)
            {
                user.FailedLoginAttempts++;
                if (user.FailedLoginAttempts >= 5)
                {
                    user.LockoutEnd = DateTime.UtcNow.AddMinutes(15);
                }
                
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
    }
}