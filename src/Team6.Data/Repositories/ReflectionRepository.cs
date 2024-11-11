using System.Data;
using System.Collections.Generic;
using System.Threading.Tasks; 
using Dapper;
using Team6.Core.Models;
using Team6.Data.Context;

namespace Team6.Data.Repositories
{

    // create a general/macro-view reflection repository to manage all-encompassing reflections 
    public class ReflectionRepository : BaseRepository
    {

        // initializes an instance of a ReflectionRepository while calling the base constructor to establish the database connection
        public ReflectionRepository(DatabaseContext context) : base(context)
        {

        }

        // get a reflection specified by Id
        public async Task<Reflection?> GetByIdAsync(int id, int userId)
        {
            using var connection  = CreateConnection();
            // define the sql query to get the reflection and its document/video 
            // we left join to include the reflection's respective document and video based on their ID
            var sql = @"SELECT Reflections.*, ReflectionDocuments.*, Videos.*
                        FROM Reflections 
                        LEFT JOIN ReflectionDocuments on Reflections.DocumentId = ReflectionDocuments.Id
                        LEFT JOIN Videos on Reflections.VideoId = Videos.Id
                        WHERE Reflections.Id = @Id AND Reflections.UserId = @UserId";
                        
            // execute the query asynchronously and map each portion of the reflection  to its object
            var reflections = await connection.QueryAsync<Reflection, ReflectionDocument, Video, Reflection>(
                sql, 
                (reflection, document, video) => {
                    // map the document and video to the reflection's params
                    reflection.Document = document;
                    reflection.Video = video;
                    return reflection;
                },
                new { Id = id, UserId = userId}, // pass the reflection Id to the sql query
                // split the results for the mapping on each Id value
                splitOn: "Id, Id"
            );

            // return the first reflection that matches the params
            return reflections.FirstOrDefault();
        }

        // iterate through and get all reflections 
        public async Task<IEnumerable<Reflection>> GetAllAsync()
        {
            var connection = CreateConnection();
            // define the sql query to get all the reflections and its document/video
            var sql = @"SELECT Reflections.*, ReflectionDocuments.*, Videos.*
                        FROM Reflections 
                        LEFT JOIN ReflectionDocuments on Reflections.DocumentId = ReflectionDocuments.Id
                        LEFT JOIN Videos on Reflections.VideoId = Videos.VideoId";
            // execute the query asynchronously and map the each portion of the reflection to its object
            return await connection.QueryAsync<Reflection, ReflectionDocument, Video, Reflection>(
                sql, 
                (reflection, document, video) => {
                    // map the document and video params
                    reflection.Document = document;
                    reflection.Video = video;
                    return reflection;
                },
                splitOn: "Id, Id"
            );
        }

        // iterate through and get all reflections for a userId
        public async Task<IEnumerable<Reflection>> GetAllForUserAsync(int userId)
        {
            var connection = CreateConnection();
            return await connection.QueryAsync<Reflection>(
                "SELECT * FROM Reflections WHERE UserId = @UserId ORDER BY CreatedAt DESC",
                new { UserId = userId}
            );
        }

        // create a new reflection entry in the database and returns its Id
        public async Task<int> CreateAsync(Reflection reflection, int userId)
        {
            var connection = CreateConnection();
            // define the sql query with the main reflection params and get the id of the last row (most recent addition to table)
            var sql = @"INSERT INTO Reflections (Title, Content, DocumentId, VideoId, UserId)
                        VALUES (@Title, @Content, @DocumentId, @VideoId, @UserId);
                        SELECT last_insert_rowid();";
            // execute the query asynchronously and return the new reflection Id
            return await connection.ExecuteScalarAsync<int>(
                sql,
                new{
                    reflection.Title,
                    reflection.Content,
                    reflection.DocumentId,
                    reflection.VideoId,
                    UserId = userId
                }
            );
        }

        // update an existing reflection
        public async Task UpdateAsync(Reflection reflection, int userId) 
        {
            using var connection = CreateConnection();
            // define the sql query to change the reflection params
            await connection.ExecuteAsync(
                @"UPDATE Reflections
                SET Title = @Title, Content = @Content, DocumentId = @DocumentId, VideoId = @VideoId, LastModified = @LastModified
                WHERE Id = @Id AND UserId = @UserId",
                new {
                    reflection.Title,
                    reflection.Content, 
                    reflection.DocumentId,
                    reflection.VideoId,
                    LastModified = DateTime.Now,
                    reflection.Id,
                    UserId = userId
                }
            );
        }

        // delete an existing reflection (specified by id)
        public async Task DeleteAsync(int id, int userId)
        {
            using var connection = CreateConnection();
            // execute the query asynchronously
            await connection.ExecuteAsync(
                "DELETE FROM Reflections WHERE Id = @Id AND UserId = @UserId",
                new { Id = id, UserId = userId}
            );
        }
    }
}