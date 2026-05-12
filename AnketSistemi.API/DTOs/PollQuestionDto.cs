using System.ComponentModel.DataAnnotations;

namespace AnketSistemi.API.DTOs
{
    public class PollQuestionDto
    {
        [Required]
        public int PollId { get; set; }

        [Required(ErrorMessage = "Soru metni boş olamaz.")]
        public string QuestionText { get; set; } = string.Empty;

        [Required]
        public int Format { get; set; }

        public bool IsMandatory { get; set; } = true;
    }
}