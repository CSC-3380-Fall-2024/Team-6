namespace Team6.Core.Models
{
    // represents a calendar event entry
    public class CalendarEvent
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public DateTime EventDate { get; set; }
        public string? Description { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now; 
    }
}


