using System.ComponentModel.DataAnnotations;

namespace AnketSistemi.API.DTOs
{
    public class PollQuestionDto
    {
        [Required]
        public int PollId { get; set; }

        [Required(ErrorMessage = "Soru metni bos olamaz.")]
        public string QuestionText { get; set; } = string.Empty;

        [Required]
        public int Format { get; set; }

        public bool IsMandatory { get; set; } = true;
    }

    public class QuestionChoiceDto
    {
        [Required]
        public int PollQuestionId { get; set; }

        [Required(ErrorMessage = "Secenek metni bos olamaz.")]
        public string ChoiceText { get; set; } = string.Empty;

        public int OrderIndex { get; set; }
    }

    public class QuestionAnswerDto
    {
        // BUG FIX #4: Alan adlari MVC tarafiyla eslesiyor
        public int QuestionId { get; set; }
        public string? TextAnswer { get; set; }
        public int? SelectedOptionId { get; set; }
    }
}
