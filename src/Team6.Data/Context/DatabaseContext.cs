using Microsoft.Data.Sqlite;
using System.Data;

namespace Team6.Data.Context
{
    // manages database connections and initialization and handles creation of tables and provides connection access
    public class DatabaseContext
    {
        private readonly string _connectionString;

       
        // initializes database with specified path where the name="dbPath" is the path to the SQlite database (just the default app.db)
        public DatabaseContext(string dbPath = "app.db")
        {
            _connectionString = $"Data Source={dbPath}";
            InitializeDatabase();
        }

        
        // initializes database and creates tables if they don't exist
        private void InitializeDatabase()
        {
            using var connection = new SqliteConnection(_connectionString);
            connection.Open();

            var command = connection.CreateCommand();
            command.CommandText = @"
                -- Users Table
                CREATE TABLE IF NOT EXISTS Users (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    Username TEXT NOT NULL UNIQUE,
                    Email TEXT NOT NULL UNIQUE,
                    PasswordHash TEXT NOT NULL,
                    CreatedAt DATETIME DEFAULT CURRENT_TIMESTAMP,
                    LastLogin DATETIME,
                    LastPasswordChange DATETIME,
                    FailedLoginAttempts INTEGER DEFAULT 0,
                    LockoutEnd DATETIME,
                    CONSTRAINT UniqueUsername UNIQUE (Username),
                    CONSTRAINT UniqueEMail UNIQUE (Email)

                );

                -- Calendar Events Table
                CREATE TABLE IF NOT EXISTS CalendarEvents (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    UserId INTEGER NOT NULL, 
                    Title TEXT NOT NULL,
                    EventDate DATETIME NOT NULL,
                    Description TEXT,
                    CreatedAt DATETIME DEFAULT CURRENT_TIMESTAMP,
                    FOREIGN KEY (UserId) REFERENCES Users(Id)
                );

                -- Reflections Table
                CREATE TABLE IF NOT EXISTS Reflections (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    UserId INTEGER NOT NULL, 
                    Title TEXT NOT NULL,
                    Content TEXT,
                    DocumentId INTEGER,
                    VideoId INTEGER,
                    CreatedAt DATETIME DEFAULT CURRENT_TIMESTAMP,
                    LastModified DATETIME DEFAULT CURRENT_TIMESTAMP,
                    FOREIGN KEY (DocumentId) REFERENCES ReflectionDocuments(Id),
                    FOREIGN KEY (VideoId) REFERENCES Videos(Id),
                    FOREIGN KEY (UserId) REFERENCES Users(Id)
                );

                -- Reflection Documents Table
                CREATE TABLE IF NOT EXISTS ReflectionDocuments (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    UserId INTEGER NOT NULL,
                    Title TEXT NOT NULL,
                    FilePath TEXT NOT NULL,
                    FileType TEXT NOT NULL,
                    FileSize INTEGER NOT NULL,
                    CreatedAt DATETIME DEFAULT CURRENT_TIMESTAMP,
                    FOREIGN KEY (UserId) REFERENCES Users(Id)
                );

                -- Videos Table
                CREATE TABLE IF NOT EXISTS Videos (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    UserId INTEGER NOT NULL,
                    Title TEXT NOT NULL,
                    Url TEXT NOT NULL,
                    Provider TEXT NOT NULL,
                    CreatedAt DATETIME DEFAULT CURRENT_TIMESTAMP,
                    FOREIGN KEY (UserId) REFERENCES Users(Id)
                );

                -- FAQ Table
                CREATE TABLE IF NOT EXISTS FAQs (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    UserId INTEGER NOT NULL,
                    Question TEXT NOT NULL,
                    Answer TEXT NOT NULL,
                    Category TEXT NOT NULL,
                    CreatedAt DATETIME DEFAULT CURRENT_TIMESTAMP,
                    FOREIGN KEY (UserId) REFERENCES Users(Id)
                );";
            command.ExecuteNonQuery();
        }

        
        // creates and returns a new database connection
        public IDbConnection CreateConnection()
        {
            return new SqliteConnection(_connectionString);
        }
    }
}