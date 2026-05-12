namespace AnketSistemi.API.DTOs
{
    public class QuestionAnswerDto
    {
        public int QuestionId { get; set; }
        public string? TextAnswer { get; set; } 
        public int? SelectedOptionId { get; set; } 
    }
}