using AnketSistemi.API.Enums;

namespace AnketSistemi.API.Models
{
    public class PollQuestion : AppBaseEntity
    {
        public string QuestionText { get; set; } = string.Empty;
        public QuestionFormat Format { get; set; }
        public bool IsMandatory { get; set; } = true;
        public int PollId { get; set; }
        public Poll? Poll { get; set; }
        public ICollection<QuestionChoice>? Choices { get; set; }
    }
}