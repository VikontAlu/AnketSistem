using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using AnketSistemi.API.Models;

namespace AnketSistemi.API.Data
{
    public class AppDbContext : IdentityDbContext<AppUser, AppRole, string>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Poll> Polls { get; set; }
        public DbSet<PollQuestion> PollQuestions { get; set; }
        public DbSet<QuestionChoice> QuestionChoices { get; set; }
        public DbSet<UserResponse> UserResponses { get; set; }
        public DbSet<PollFeedback> PollFeedbacks { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<PollFeedback>()
                .HasOne(f => f.User)
                .WithMany()
                .HasForeignKey(f => f.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<UserResponse>()
                .HasOne(ur => ur.PollQuestion)
                .WithMany()
                .HasForeignKey(ur => ur.PollQuestionId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<UserResponse>()
                .HasOne(ur => ur.AppUser)
                .WithMany()
                .HasForeignKey(ur => ur.AppUserId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
