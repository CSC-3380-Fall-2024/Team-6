using System.Data;
using System.Collections.Generic;
using System.Threading.Tasks; 
using Dapper;
using Team6.Core.Models;
using Team6.Data.Context;

namespace Team6.Data.Repositories
{

    // create a repository to manage operations just for videos in a reflection
    public class VideoRepository : BaseRepository
    {

        // initialize an instance of the VideoRepository while calling the base constructor to establish the database connection
        public VideoRepository(DatabaseContext context) : base(context)
        {

        }
        
        // get a video specified by Id
        public async Task<Video?> GetByIdAsync(int id)
        {
            using var connection = CreateConnection();
            // execute the query asynchronously and get the first video where the Id matches
            return await connection.QueryFirstOrDefaultAsync<Video>(
                "SELECT * FROM Videos WHERE Id = @Id",
                new { Id = id }
            );
        }

        // iterate through and get all of the videos
        public async Task<IEnumerable<Video>> GetAllAsync()
        {
            using var connection = CreateConnection();
            // execute the query asynchronously and get a list of all the videos
            return await connection.QueryAsync<Video>(
                "SELECT * FROM Videos"
            );
        }

        // add a new video to the database and return the video Id
        public async Task<int> CreateAsync(Video video)
        {
            using var connection = CreateConnection();
            // define the sql query with the main video params
            var sql = @"INSERT INTO Videos (Title, Url, Provider)
                        VALUES (@Title, @Url, @Provider);
                        SELECT last_insert_rowid();";
            // execute the query asynchronously and return the new video Id
            return await connection.ExecuteScalarAsync<int>(sql, video);
        }

        // update an existing video
        public async Task UpdateAsync(Video video)
        {
            using var connection = CreateConnection();
            // define the sql query to change the document params
            var sql = @"UPDATE Videos
                        SET Title = @Title, Url = @Url, Provider = @Provider
                        WHERE Id = @Id";
            // execute the query asychronously 
            await connection.ExecuteAsync(sql, video);
        }

        // delete an existing video (specified by Id)
        public async Task DeleteAsync(int id)
        {
            using var connection = CreateConnection();
            // execute the query asynchronously
            await connection.ExecuteAsync(
                "DELETE FROM Videos WHERE Id = @Id", new { Id = id}
            );
        }
    }
}