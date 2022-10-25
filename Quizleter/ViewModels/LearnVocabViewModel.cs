using System.Collections.Generic;
using Quizleter.Models;

namespace Quizleter.ViewModels
{
    public class LearnVocabViewModel
    {
        public long Id { get; set; }

        public long LearnsetId { get; set; }

        public string Definition { get; set; }

        public long VocabId { get; set; }

        public string Input { get; set; }
    }
}
