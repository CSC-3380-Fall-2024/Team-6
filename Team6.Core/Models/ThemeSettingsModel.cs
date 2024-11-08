namespace Team6.Core.Models
{

    // represents a user's theme setting (dark v light mode)
    public class ThemeSetting
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public bool IsDarkMode { get; set;}
        public DateTime LastModified { get; set; } = DateTime.Now;
    }
}