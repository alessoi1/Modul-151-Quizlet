using Microsoft.EntityFrameworkCore;
using Quizleter.Data;
using Quizleter.Models;
using Quizleter.ViewModels;
using System;
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

        public async Task<IEnumerable<Learnset>> GetLearnsetsByUserAsync(string username)
        {
            return await _context.Learnset.Where(l => l.CreatorUsername.Equals(username)).ToListAsync();
        }

        public async Task<IEnumerable<Skill>> GetLearnVocabByLernsetId(long id, string username)
        {
            var learnVocabList = new List<LearnVocabViewModel>();
            var vocabs = _context.Vocab.Where(v => v.LearnsetId == id)
                                       .ToList();

            if (vocabs is null)
            {
                return null;
            }

            foreach (var voc in vocabs)
            {
                var skill = _context.Skill.FirstOrDefault(s => string.Equals(s.Username, username)
                                                 && s.VocabId == voc.Id);
                if (skill is null)
                {
                    await _context.Skill.AddAsync(new Skill
                    {
                        Username = username,
                        SkillLevel = 0,
                        Vocab = voc,
                        VocabId = voc.Id
                    });

                    await _context.SaveChangesAsync();
                }
            }

            return GetSkillsByLearnset(id);
        }

        private IEnumerable<Skill> GetSkillsByLearnset(long id)
        {
            return _context.Skill.Where(s => s.Vocab.LearnsetId == id)
                                 .ToList();
        }

        public Skill GetRandomSkill(IEnumerable<Skill> learnVocabList)
        {
            var lowestSkillValue = learnVocabList.Min(s => s.SkillLevel);

            var rand = new Random();
            var voacbWithLowestValue = learnVocabList
                .OrderBy(x => rand.NextDouble())
                .FirstOrDefault(s => s.SkillLevel == lowestSkillValue || s.SkillLevel == lowestSkillValue + 1 && s.SkillLevel <= 4);

            return voacbWithLowestValue;
        }
    }
}
