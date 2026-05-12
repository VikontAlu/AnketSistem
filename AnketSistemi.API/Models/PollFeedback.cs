namespace AnketSistemi.API.Models
{
    public class PollFeedback : AppBaseEntity
    {
        public int PollId { get; set; }
        public Poll Poll { get; set; }

        public string UserId { get; set; } // Rol modeldeki AppUserId yerine bunu kullandık
        public AppUser User { get; set; }

        public byte Score { get; set; } // Rating yerine Score (1-5 arası)
        public string UserComment { get; set; } // Comment yerine UserComment
    }
}