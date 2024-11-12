using System.Data;
using System.Collections.Generic;
using System.Threading.Tasks; 
using Dapper;
using Team6.Core.Models;
using Team6.Data.Context;

namespace Team6.Data.Repositories
{

    // create a repository to manage operations for just the documents in a reflection
    public class ReflectionDocumentRepository : BaseRepository
    {

        // initialize an instance of ReflectionDocumentRepository while calling the base constructor to establish the database connection
        public ReflectionDocumentRepository(DatabaseContext context) : base(context)
        {

        }

        // get a document specified by Id
        public async Task<ReflectionDocument?> GetByIdAsync(int id)
        {
            using var connection = CreateConnection();
            // execute the query asynchronously and get the first document where the Id matches
            return await connection.QueryFirstOrDefaultAsync<ReflectionDocument>(
                "SELECT * FROM ReflectionDocuments WHERE Id = @Id", 
                new {Id = id }
            );
        }

        // iterate through and get all of the documents
        public async Task<IEnumerable<ReflectionDocument>> GetAllAsync()
        {
            using var connection = CreateConnection();
            // execute the query asynchronously and get a list of all the documents
            return await connection.QueryAsync<ReflectionDocument>(
                "SELECT * FROM ReflectionDocuments"
            );
        }

        // add a new document to the database and return the document Id
        public async Task<int> CreateAsync(ReflectionDocument document)
        {
            using var connection = CreateConnection();
            // define the sql query with the main document params
            var sql = @"INSERT INTO ReflectionDocuments (Title, FilePath, FileType, FileSize)
                        VALUES (@Title, @FilePath, @FileType, @FileSize);
                        SELECT last_insert_rowid();";
            // execute the query asynchronously and return the new document Id
            return await connection.ExecuteScalarAsync<int>(sql, document);
        }

        // update an existing document 
        public async Task UpdateAsync(ReflectionDocument document)
        {
            using var connection = CreateConnection();
            // define the sql query to change the document params
            var sql = @"UPDATE ReflectionDocuments
                        SET Title = @Title, FilePath = @FilePath, FileType = @FileType, FileSize = @FileSize
                        WHERE Id = @Id";
            // execute the query asynchronously
            await connection.ExecuteAsync(sql, document);
        }

        // delete an existing document (specified by Id)
        public async Task DeleteAsync(int id)
        {
            using var connection = CreateConnection();
            // execute the query asynchronously
            await connection.ExecuteAsync(
                "DELETE FROM ReflectionDocuments WHERE Id = @Id", new { Id = id }
            );
        }
    }

   
}