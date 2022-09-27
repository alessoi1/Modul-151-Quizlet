using Quizleter.Data;
using Quizleter.Models;
using System.Collections.Generic;
using System.Linq;

namespace Quizleter.Services.Learnsets.Impl
{
    public class LearnsetService : ILearnsetService
    {
        private readonly QuizleterContext _context;

        public LearnsetService(QuizleterContext context)
        {
            _context = context;
        }

        public IEnumerable<Learnset> GetLearnsetsByUser(string email)
        {
            return _context.Learnset.Where(l => l.CreatorEmail.Equals(email));
        }
    }
}
