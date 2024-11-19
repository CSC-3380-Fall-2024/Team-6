namespace Team6.Core.Models
{
   public class User
   {
       public int Id { get; set; }
       public string Username { get; set; } = string.Empty;
       public string Email { get; set; } = string.Empty;
       public string PasswordHash { get; set; } = string.Empty;
       public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
       public DateTime? LastLogin { get; set; }
       public DateTime? LastPasswordChange { get; set; }
       public int FailedLoginAttempts { get; set; }
       public DateTime? LockoutEnd { get; set; }

       // navigation properties
       public List<Note> Notes { get; set; } = new();
       public List<CalendarEvent>? CalendarEvents { get; set; }
       public List<Reflection>? Reflections { get; set; }
   }
}