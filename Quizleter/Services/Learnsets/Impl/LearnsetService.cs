using Microsoft.EntityFrameworkCore;
using Quizleter.Data;
using Quizleter.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Quizleter.Services.Learnsets.Impl
{
    public class LearnsetService : ILearnsetService
    {
        private readonly QuizleterContext _context;

        public LearnsetService(QuizleterContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Learnset>> GetAllLearnsetsAsync()
        {
            return await _context.Learnset.ToListAsync();
        }

        public async Task<IEnumerable<Learnset>> GetLearnsetsByUserAsync(string email)
        {
            return await _context.Learnset.Where(l => l.CreatorEmail.Equals(email)).ToListAsync();
        }
    }
}
