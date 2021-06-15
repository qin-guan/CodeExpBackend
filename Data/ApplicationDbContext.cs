using CodeExpBackend.Models;
using Microsoft.EntityFrameworkCore;

namespace CodeExpBackend.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>()
                .HasMany(u => u.Classrooms)
                .WithMany(c => c.Users);

            modelBuilder.Entity<User>()
                .HasMany(u => u.AdminClassrooms)
                .WithOne(c => c.AdminUser);
        }

        public DbSet<Classroom> Classrooms { get; set; }
        public DbSet<Quiz> Quizzes { get; set; }
        public DbSet<Question> Questions { get; set; }
        public DbSet<McqQuestion> McqQuestions { get; set; }
        public DbSet<ShortAnswerQuestion> ShortAnswerQuestions { get; set; }
        public DbSet<OpenEndedQuestion> OpenEndedQuestions { get; set; }
        public DbSet<InfoSlideQuestion> InfoSlideQuestions { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Team> Teams { get; set; }
    }
}