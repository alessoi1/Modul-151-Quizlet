using Quizleter.Models;
using Quizleter.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Quizleter.Services.Learnsets
{
    public interface ILearnsetService
    {
        Task<IEnumerable<Learnset>> GetAllLearnsetsAsync();
        Task<IEnumerable<Learnset>> GetLearnsetsByUserAsync(string email);
        Task<IEnumerable<Skill>> GetLearnVocabByLernsetId(long id, string username);
        Skill GetRandomSkill(IEnumerable<Skill> learnVocabList);
    }
}