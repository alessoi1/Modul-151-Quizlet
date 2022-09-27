using Quizleter.Models;
using System.Collections.Generic;

namespace Quizleter.Services.Learnsets
{
    public interface ILearnsetService
    {
        IEnumerable<Learnset> GetLearnsetsByUser(string email);
    }
}