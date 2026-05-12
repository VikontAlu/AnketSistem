namespace AnketSistemi.API.Models
{
    public class UserResponse : AppBaseEntity
    {
        public string? WrittenAnswer { get; set; } 

 
        public int? SelectedChoiceId { get; set; }
        public QuestionChoice? SelectedChoice { get; set; }

        public int PollId { get; set; }
        public Poll? Poll { get; set; }

        public int PollQuestionId { get; set; }
        public PollQuestion? PollQuestion { get; set; }

        public string AppUserId { get; set; } = string.Empty; 
        public AppUser? AppUser { get; set; }
    }
}