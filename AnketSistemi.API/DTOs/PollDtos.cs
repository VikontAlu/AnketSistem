using System.ComponentModel.DataAnnotations;

namespace AnketSistemi.API.DTOs
{
    public class PollCreateDto
    {
        [Required(ErrorMessage = "Anket basligi zorunludur.")]
        public string Title { get; set; } = string.Empty;
        public string? Detail { get; set; }

        [Required]
        public DateTime ExpireDate { get; set; }
    }

    public class PollViewDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string? Detail { get; set; }
        public DateTime CreatedAt { get; set; }
        public int QuestionCount { get; set; }
    }
}
