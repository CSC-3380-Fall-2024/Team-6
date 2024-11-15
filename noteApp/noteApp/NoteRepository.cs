using System;
using System.Data;
using Microsoft.Data.Sqlite;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace noteApp.Data
{
    public class NoteRepository
    {
        private readonly string _connectionString = "Data Source=Notes.db;Version=3;";

        public NoteRepository()
        {
            InitializeDatabase();
        }

        private void InitializeDatabase()
        {
            using (var connection = new SqliteConnection(_connectionString))
            {
                connection.Open();
                string createTableQuery = @"
                    CREATE TABLE IF NOT EXISTS Notes (
                        Id INTEGER PRIMARY KEY AUTOINCREMENT,
                        Title TEXT NOT NULL,
                        Content TEXT NOT NULL
                    )";
                using (var command = new SqliteCommand(createTableQuery, connection))
                {
                    command.ExecuteNonQuery();
                }
            }
        }

        public async Task<List<Note>> GetAllNotesAsync()
        {
            var notes = new List<Note>();
            using (var connection = new SqliteConnection(_connectionString))
            {
                await connection.OpenAsync();
                var selectQuery = "SELECT Id, Title, Content FROM Notes";
                using (var command = new SqliteCommand(selectQuery, connection))
                using (var reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        notes.Add(new Note
                        {
                            Id = reader.GetInt32(0),
                            Title = reader.GetString(1),
                            Content = reader.GetString(2)
                        });
                    }
                }
            }
            return notes;
        }

        public async Task AddNoteAsync(Note note)
        {
            using (var connection = new SqliteConnection(_connectionString))
            {
                await connection.OpenAsync();
                var insertQuery = "INSERT INTO Notes (Title, Content) VALUES (@Title, @Content)";
                using (var command = new SqliteCommand(insertQuery, connection))
                {
                    command.Parameters.AddWithValue("@Title", note.Title);
                    command.Parameters.AddWithValue("@Content", note.Content);
                    await command.ExecuteNonQueryAsync();
                }
            }
        }

        public async Task UpdateNoteAsync(int id, string title, string content)
        {
            using (var connection = new SqliteConnection(_connectionString))
            {
                await connection.OpenAsync();
                var updateQuery = "UPDATE Notes SET Title = @Title, Content = @Content WHERE Id = @Id";
                using (var command = new SqliteCommand(updateQuery, connection))
                {
                    command.Parameters.AddWithValue("@Id", id);
                    command.Parameters.AddWithValue("@Title", title);
                    command.Parameters.AddWithValue("@Content", content);
                    await command.ExecuteNonQueryAsync();
                }
            }
        }

        public async Task DeleteNoteAsync(int id)
        {
            using (var connection = new SqliteConnection(_connectionString))
            {
                await connection.OpenAsync();
                var deleteQuery = "DELETE FROM Notes WHERE Id = @Id";
                using (var command = new SqliteCommand(deleteQuery, connection))
                {
                    command.Parameters.AddWithValue("@Id", id);
                    await command.ExecuteNonQueryAsync();
                }
            }
        }
    }
    
    public class Note
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
    }
}
