namespace Team6.Core.Models
{
    // represents a user account
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime? LastLogin { get; set; }
        public DateTime? LastPasswordChange { get; set; }
        public int numFailedLogins { get; set; }
        public DateTime? LockedOutEnd { get; set; }

        // navigation 
        public List<CalendarEvent>? CalendarEvents { get; set; }
        public List<Reflection>? Reflections { get; set; }
        public ThemeSetting? ThemeSetting { get; set; }
    }
}