namespace AnketSistemi.API.Models
{
    public class Poll : AppBaseEntity
    {
        public string Title { get; set; }

        // Description yerine Controller'ın beklediği Detail ismini kullanıyoruz
        public string Detail { get; set; }

        // CreatorId yerine Controller'ın beklediği AppUserId ismini kullanıyoruz
        public string AppUserId { get; set; }
        public AppUser AppUser { get; set; }

        // Controller'ın aradığı ama bizim eklemeyi unuttuğumuz Bitiş Tarihi alanı
        public DateTime ExpireDate { get; set; }

        // Bize özgü eklediğimiz çaktırmadan duran özellikler
        public bool IsActive { get; set; } = true;
        public int TotalViews { get; set; } = 0;

        public ICollection<PollQuestion> Questions { get; set; }
        public ICollection<PollFeedback> Feedbacks { get; set; }
    }
}