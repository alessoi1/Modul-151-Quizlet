using Quizleter.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Quizleter.Services.Learnsets
{
    public interface ILearnsetService
    {
        Task<IEnumerable<Learnset>> GetAllLearnsetsAsync();
        Task<IEnumerable<Learnset>> GetLearnsetsByUserAsync(string email);
    }
}