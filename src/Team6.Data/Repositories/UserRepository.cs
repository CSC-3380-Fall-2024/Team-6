using System.Data;
using System.Collections.Generic;
using System.Threading.Tasks; 
using Dapper;
using Team6.Core.Models;
using Team6.Data.Context;

namespace Team6.Data.Repositories
{
    

    // create a repository to manage operations for user accounts and authentication
    public class UserRepository : BaseRepository
    {

        private readonly PasswordHasher _passwordHasher;

        // initialize an instance of UserRepository while calling the base constructor to establish the database connection
        public UserRepository(DatabaseContext context) : base(context)
        {
            _passwordHasher = new PasswordHasher();
        }

        //get a user by their ID 
        public async Task<User?> GetByIdAsync(int id)
        {
            using var connection = CreateConnection();
            //execture the query asynchronously and get the first user where the username matches
            return await connection.QueryFirstOrDefaultAsync<User>(
                "SELECT * FROM Users WHERE Id = @Id",
                new { Id = id }
            );
        }

        //get a user by their username 
        public async Task<User?> GetByUsernameAsync(string username)
        {
            using var connection = CreateConnection();
            //execture the query asynchronously and get the first user where the username matches
            return await connection.QueryFirstOrDefaultAsync<User>(
                "SELECT * FROM Users WHERE Username = @Username",
                new { Username = username }
            );
        }

        // get a user by their email
        public async Task<User?> GetByEmailAsync(string email)
        {
            using var connection = CreateConnection();
            //execture the query asynchronously and get the first user where the email matches
            return await connection.QueryFirstOrDefaultAsync<User>(
                "SELECT * FROM Users WHERE Email = @Email",
                new { Email = email }
            );
        }

        // create a new user account
        public async Task<(bool Success, User? User, string[] Errors)> CreateUserAsync(string username, string email, string password)
        {
            // hash password
            var (success, hashedPassword, errors) = _passwordHasher.HashPassword(password);
            if (!success)
                return (false, null, errors);

            using var connection = CreateConnection();
            
            // check if username/email exists
            if (await connection.ExecuteScalarAsync<bool>(
                "SELECT COUNT(1) FROM Users WHERE Username = @Username OR Email = @Email",
                new { Username = username, Email = email }))
            {
                return (false, null, new[] { "Username or email already exists" });
            }

            var user = new User
            {
                Username = username,
                Email = email,
                PasswordHash = hashedPassword
            };

            var userId = await connection.ExecuteScalarAsync<int>(@"
                INSERT INTO Users (Username, Email, PasswordHash, CreatedAt)
                VALUES (@Username, @Email, @PasswordHash, @CreatedAt);
                SELECT last_insert_rowid();",
                user);

            user.Id = userId;
            return (true, user, Array.Empty<string>());
        }


        public async Task<(bool Success, User? User, string[] Errors)> ValidateLoginAsync(string username, string password)
        {
            using var connection = CreateConnection();
            var user = await connection.QueryFirstOrDefaultAsync<User>(
                "SELECT * FROM Users WHERE Username = @Username",
                new { Username = username });

            if (user == null)
                return (false, null, new[] { "Invalid username or password" });

            // check for lockout
            if (user.LockoutEnd.HasValue && user.LockoutEnd > DateTime.UtcNow)
                return (false, null, new[] { "Account is locked. Try again later." });

            // verify password
            var (success, needsRehash, newHash) = _passwordHasher.VerifyPassword(
                user.PasswordHash, 
                password);

            if (!success)
            {
                // handle failed attempt
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

            // update hash if needed
            if (needsRehash)
            {
                await connection.ExecuteAsync(
                    "UPDATE Users SET PasswordHash = @Hash WHERE Id = @Id",
                    new { Hash = newHash, user.Id });
            }

            return (true, user, Array.Empty<string>());
        }

        public async Task<bool> ChangePasswordAsync(int userId, string currentPassword, string newPassword)
        {
            using var connection = CreateConnection();
            var user = await connection.QueryFirstOrDefaultAsync<User>(
                "SELECT * FROM Users WHERE Id = @Id",
                new { Id = userId });

            if (user == null) return false;

            // verify current password
            var (success, _, _) = _passwordHasher.VerifyPassword(
                user.PasswordHash, 
                currentPassword);

            if (!success) return false;

            // hash and save new password
            var (hashSuccess, hashedPassword, errors) = _passwordHasher.HashPassword(newPassword);
            if (!hashSuccess) return false;

            await connection.ExecuteAsync(@"
                UPDATE Users 
                SET PasswordHash = @Hash, 
                    LastPasswordChange = @Now 
                WHERE Id = @Id",
                new { 
                    Hash = hashedPassword, 
                    Now = DateTime.UtcNow, 
                    Id = userId 
                });

            return true;
        }

    }
}