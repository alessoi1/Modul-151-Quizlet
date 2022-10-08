using Quizleter.Models;
using System.Collections.Generic;

namespace Quizleter.ViewModels
{
    public class TestLearnsetViewModel
    {
        public int Index { get; set; }
        public List<Vocab> Vocabulary { get; set; }
        public string Input { get; set; }
    }
}
