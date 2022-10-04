using Quizleter.Models;
using System.Collections.Generic;

namespace Quizleter.ViewModels
{
    public class LearnsetsViewModel
    {
        public List<Learnset> OwnedLearnsets { get; set; }
        public List<Learnset> OtherLearnsets { get; set; }

        public LearnsetsViewModel()
        {
            OwnedLearnsets = new List<Learnset>();
            OtherLearnsets = new List<Learnset>();
        }
    }
}
