using System.ComponentModel.DataAnnotations;

namespace AnketSistemi.API.DTOs
{
    public class QuestionChoiceDto
    {
        [Required]
        public int PollQuestionId { get; set; }

        [Required(ErrorMessage = "Seçenek metni boş olamaz.")]
        public string ChoiceText { get; set; } = string.Empty;

        public int OrderIndex { get; set; }
    }
}