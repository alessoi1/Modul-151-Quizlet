using System.Collections.Generic;
using System.ComponentModel;

namespace Quizleter.ViewModels
{
    public class TestResultViewModel
    {
        public List<TestVocabViewModel> Vocabulary { get; set; }

        [DisplayName("Punkte")]
        public int Points { get; set; }

        public int Percentage { get; set; }
    }
}
