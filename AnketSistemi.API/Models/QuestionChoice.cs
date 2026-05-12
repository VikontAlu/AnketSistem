namespace AnketSistemi.API.Models
{
    public class QuestionChoice : AppBaseEntity
    {
        public string ChoiceText { get; set; } = string.Empty;
        public int OrderIndex { get; set; } 

        public int PollQuestionId { get; set; }
        public PollQuestion? PollQuestion { get; set; }
    }
}