using Quizleter.Models;
using System.Collections.Generic;

namespace Quizleter.ViewModels
{
    public class CreateLearnsetViewModel
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public IEnumerable<Vocab> Vocabulary { get; set; }
    }
}
