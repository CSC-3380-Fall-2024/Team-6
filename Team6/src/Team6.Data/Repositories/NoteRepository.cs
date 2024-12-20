using System.Data;
using System.Collections.Generic;
using System.Threading.Tasks; 
using Dapper;
using Team6.Core.Models;
using Team6.Data.Context;

namespace Team6.Data.Repositories
{
    public class NoteRepository : BaseRepository
    {
        public NoteRepository(DatabaseContext context) : base(context)
        {
        }

        // Get notes for a specific user
        public async Task<List<Note>> GetUserNotesAsync(int userId)
        {
            using var connection = CreateConnection();
            var notes = await connection.QueryAsync<Note>(
                "SELECT * FROM Notes WHERE UserId = @UserId",
                new { UserId = userId }
            );
            return notes.ToList();
        }

        // Get a note by ID for a specific user
        public async Task<Note?> GetByIdAsync(int id, int userId)
        {
            using var connection = CreateConnection();
            return await connection.QueryFirstOrDefaultAsync<Note>(
                "SELECT * FROM Notes WHERE Id = @Id AND UserId = @UserId",
                new { Id = id, UserId = userId }
            );
        }

        // Create a new note
        public async Task<int> CreateNoteAsync(Note note)
        {
            using var connection = CreateConnection();
            return await connection.ExecuteScalarAsync<int>(@"
                INSERT INTO Notes (UserId, Title, Content, CreatedAt)
                VALUES (@UserId, @Title, @Content, @CreatedAt);
                SELECT last_insert_rowid();",
                note
            );
        }

        // Update an existing note
        public async Task UpdateNoteAsync(Note note)
        {
            using var connection = CreateConnection();
            await connection.ExecuteAsync(@"
                UPDATE Notes 
                SET Title = @Title, Content = @Content 
                WHERE Id = @Id AND UserId = @UserId",
                note
            );
        }

        // Delete a note by its ID for a specific user
        public async Task DeleteNoteAsync(int id, int userId)
        {
            using var connection = CreateConnection();
            await connection.ExecuteAsync(
                "DELETE FROM Notes WHERE Id = @Id AND UserId = @UserId",
                new { Id = id, UserId = userId }
            );
        }
    }
}
