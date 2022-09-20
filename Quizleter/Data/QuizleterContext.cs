using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Quizleter.Models;

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
    }
}
