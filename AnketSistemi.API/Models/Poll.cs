namespace AnketSistemi.API.Models
{
    public class Poll : AppBaseEntity
    {
        public string Title { get; set; } = string.Empty;
        public string? Detail { get; set; }
        public DateTime ExpireDate { get; set; } // Bitiş tarihi

        // Anketi oluşturan kullanıcı
        public string AppUserId { get; set; } = string.Empty;
        public AppUser? AppUser { get; set; }

        public ICollection<PollQuestion>? Questions { get; set; }
    }
}