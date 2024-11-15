namespace noteApp;

    public class Note
    {
        public int Id { get; set; } // Unique identifier for the note.
        public string Title { get; set; } // Title of the note.
        public string Content { get; set; } // Content/body of the note.
        
        public Note()
        {
            Title = string.Empty; // Default value
            Content = string.Empty; // Default value
        }
    }
