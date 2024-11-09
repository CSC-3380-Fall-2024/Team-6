namespace Team6.Core.Models
{

    // represents a video embedded into the reflection viewer
    public class Video
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Url { get; set; }
        public string Provider { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
    }
}