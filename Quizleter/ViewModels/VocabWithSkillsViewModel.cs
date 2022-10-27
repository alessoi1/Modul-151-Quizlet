using Quizleter.Models;

namespace Quizleter.ViewModels
{
    public class VocabWithSkillsViewModel
    {
        public long Id { get; set; }

        public Vocab Vocab { get; set; }

        public long Skill { get; set; }
    }
}
