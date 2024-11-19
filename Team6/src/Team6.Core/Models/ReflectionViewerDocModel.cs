namespace Team6.Core.Models
{

    // represents a document entry into the reflection viewer
    public class ReflectionDocument 
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string FilePath { get; set; } = string.Empty;
        public string FileType { get; set; } = string.Empty;
        public int FileSize { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}