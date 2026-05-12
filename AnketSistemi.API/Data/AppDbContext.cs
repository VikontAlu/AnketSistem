using AnketSistemi.API.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AnketSistemi.API.Data
{
    public class AppDbContext : IdentityDbContext<AppUser, AppRole, string>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Poll> Polls { get; set; }
        public DbSet<PollQuestion> PollQuestions { get; set; }
        public DbSet<QuestionChoice> QuestionChoices { get; set; }
        public DbSet<UserResponse> UserResponses { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

  
            builder.Entity<UserResponse>()
                .HasOne(ur => ur.Poll)
                .WithMany()
                .HasForeignKey(ur => ur.PollId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<UserResponse>()
                .HasOne(ur => ur.PollQuestion)
                .WithMany()
                .HasForeignKey(ur => ur.PollQuestionId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}