using System.Collections.Generic;

namespace Quizleter.ViewModels
{
    public class EvaluationViewModel
    {
        public List<VocabWithSkillsViewModel> VocabsWithSkills { get; set; }
        public int Percentage { get; set; }
    }
}
