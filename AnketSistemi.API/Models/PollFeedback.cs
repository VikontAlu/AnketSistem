namespace AnketSistemi.API.Models
{
    public class PollFeedback : AppBaseEntity
    {
        public int PollId { get; set; }
        public Poll? Poll { get; set; }
        public string UserId { get; set; } = string.Empty;
        public AppUser? User { get; set; }
        public byte Score { get; set; }
        public string UserComment { get; set; } = string.Empty;
    }
}