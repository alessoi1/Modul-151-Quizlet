using Microsoft.EntityFrameworkCore;
using Quizleter.Models;
using Quizleter.ViewModels;

namespace Quizleter.Data
{
    public class QuizleterContext : DbContext
    {
        public QuizleterContext (DbContextOptions<QuizleterContext> options)
            : base(options)
        {
        }

        public DbSet<Learnset> Learnset { get; set; }

        public DbSet<Vocab> Vocab { get; set; }

        public DbSet<Skill> Skill { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Skill>()
                .HasKey(b => new { b.Username, b.VocabId });
        }

        public DbSet<Quizleter.ViewModels.LearnVocabViewModel> LearnVocab { get; set; }

        public DbSet<Quizleter.ViewModels.VocabWithSkillsViewModel> VocabWithSkillsViewModel { get; set; }
    }
}
