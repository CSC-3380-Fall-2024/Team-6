namespace Team6.Core.Models{

    // represents a reflection entry, either video or document 
    public class Reflection 
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string? Content { get; set; }
        public int? DocumentId { get; set; }
        public int? VideoId { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime LastModified { get; set; } = DateTime.Now;
        public ReflectionDocument? Document { get; set; }
        public Video? Video { get; set; }
    }
}