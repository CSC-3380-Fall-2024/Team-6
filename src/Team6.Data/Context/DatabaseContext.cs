using Microsoft.Data.Sqlite;
using System.Data;

namespace Team6.Data.Context
{
    
    // manage database connections and initalization
    // handle creation of tables and provide connection access
    public class DatabaseContext
    {
        private readonly string _connectionString; 
        
        // constructor to initialize the database with a specified path where the param name="dbPath" is the path to the SQlite database (just the default app.db)
        public DatabaseContext(string dbPath = "app.db")
        {
            _connectionString = $"Data Source={dbPath}";
            InitializeDatabase();
        }

        // initalize database and create tables if they don't already exist
        private void InitializeDatabase()
        {
            using var connection = new SqliteConnection(_connectionString);
            connection.Open();

            // create tables
            var command = connection.CreateCommand(); 
            command.CommandText = @"
                -- Calendar Events Table
                -- Stores calendar entries with their dates, titles, and details
                CREATE TABLE IF NOT EXISTS CalendarEvents (
                    EventID INTEGER PRIMARY KEY AUTOINCREMENT,
                    Title TEXT NOT NULL,
                    EventDate DATETIME NOT NULL,
                    Description TEXT,
                    CreatedAt DATETIME DEFAULT CURRENT_TIMESTAMP
                );

                -- Color Theme Settings Table 
                -- Stores the booleans and information needed to manage the light mode/dark mode
                    CREATE TABLE IF NOT EXISTS ThemeSettings (
                        SettingID INTEGER PRIMARY KEY AUTOINCREMENT,
                        UserID TEXT NOT NULL,
                        IsDarkMode BOOLEAN DEFAULT 0,
                        LastModified DATETIME DEFAULT CURRENT_TIMESTAMP
                );

                -- Reflection Viewer Table
                -- Main Table for reflection viewer entries and their item details
                CREATE TABLE IF NOT EXISTS ReflectionViewer (
                    ReflectionID INTEGER PRIMARY KEY AUTOINCREMENT,
                    Title TEXT NOT NULL,
                    Content Text,
                    DocumentID INTEGER,
                    VideoID INTEGER, 
                    CreatedAt DATETIME DEFAULT CURRENT_TIMESTAMP, 
                    LastModified DATETIME DEFAULT CURRENT_TIMESTAMP,
                    FOREIGN KEY (DocumentID) REFERENCES ReflectionViewerDocuments(DocumentID),
                    FOREIGN KEY (VideoID) REFERENCES VideoEmbeddings(VideoID)
                );

                -- Reflection Viewer Documents Table
                -- Main table for reflection entries, links to documents and videos
                CREATE TABLE IF NOT EXISTS ReflectionViewerDocuments (
                    ReflectionID INTEGER PRIMARY KEY AUTOINCREMENT,
                    Title TEXT NOT NULL,
                    FilePath TEXT NOT NULL,
                    FileType TEXT NOT NULL, -- Word doc, pdf, other, etc
                    FileSize INTEGER NOT NULL, 
                    CreatedAt DATETIME DEFAULT CURRENT_TIMESTAMP
                );

                -- Video Embeddings Table
                -- Stores video URLs and other data for embedded videos
                CREATE TABLE IF NOT EXISTS VideoEmbeddings (
                    VideoID INTEGER PRIMARY KEY AUTOINCREMENT,
                    Title TEXT NOT NULL,
                    URL TEXT NOT NULL,
                    VideoSource TEXT NOT NULL, -- YouTube, etc
                    CreatedAt DATETIME DEFAULT CURRENT_TIMESTAMP
                );

                -- FAQ Table
                -- Stores questions and answers
                CREATE TABLE IF NOT EXISTS FAQs (
                    FAQID INTEGER PRIMARY KEY AUTOINCREMENT,
                    Question TEXT NOT NULL,
                    Answer TEXT NOT NULL, 
                    Category TEXT NOT NULL, 
                    CreatedAt DATETIME DEFAULT CURRENT_TIMESTAMP
                );
            ";
            
            command.ExecuteNonQuery();
                
        }

        // create and return the new database connection
        public IDbConnection CreateConnection()
        {
            return new SqliteConnection(_connectionString);
        }
    }
}