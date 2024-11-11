using System.Data;
using System.Collections.Generic;
using Team6.Data.Context;

namespace Team6.Data.Repositories
{

    // create a repository to manage operations for user accounts and authentication
    public class UserRepository : BaseRepository
    {

        // initialize an instance of UserRepository while calling the base constructor to establish the database connection
        public UserRepository(DatabaseContext context) : base(context)
        {

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
        public async Task<int> CreateAsync(User user)
        {
            using var connection = CreateConnection();
            // define the sql query with main user params
            var sql = @"INSERT INTO Users (Username, Email, PasswordHash)
                        VALUES (@Username, @Email, @PasswordHash);
                        SELECT last_insert_rowid();";
            // execute the query asynchronously and return the new user Id
            return await connection.ExecuteScalarAsync<int>(sql, user);
        }

        // check if username is available
        public async Task<bool> IsUsernameAvailableAsync(string username)
        {
            using var connection = CreateConnection();
            // get count of all matching usernames to see if >0
            await connection.ExecuteScalarAsync<bool>(
                "SELECT COUNT(1) FROM Users WHERE Username = @Username",
                new { Username = username }
            );
            return !exists;
            
        }

        // update the user's login time 
        public async Task UpdateLastLoginAsync(int id) 
        {
            using var connection = CreateConnection();
            // update the login timestamp
            await connection.ExecuteAsync(
                "UPDATE Users SET LastLogin = @Now WHERE Id = @Id",
                new { Now = DateTime.now, Id = id }
            );
        }

    }
}