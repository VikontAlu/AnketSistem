namespace AnketSistemi.API.Models
{
    public class Poll : AppBaseEntity
    {
        public string Title { get; set; } = string.Empty;
        public string Detail { get; set; } = string.Empty;
        public string AppUserId { get; set; } = string.Empty;
        public AppUser? AppUser { get; set; }
        public DateTime ExpireDate { get; set; }
        public int TotalViews { get; set; } = 0;
        public ICollection<PollQuestion>? Questions { get; set; }
        public ICollection<PollFeedback>? Feedbacks { get; set; }
    }
}