namespace Team6.Core.Models 
{

    // represents a FAQ entry
    public class FAQ 
    {
        public int Id { get; set; }
        public string Question { get; set; } = string.Empty;
        public string Answer { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.Now();
    }

}