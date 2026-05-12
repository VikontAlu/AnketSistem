using Microsoft.AspNetCore.Identity;

namespace AnketSistemi.API.Models
{
    public class AppUser : IdentityUser
    {
        public string FullName { get; set; } = string.Empty;

      
        public ICollection<Poll>? Polls { get; set; }
    }
}