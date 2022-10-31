using System.ComponentModel;

namespace Quizleter.ViewModels
{
    public class LearnsetDetailsViewModel
    {
        public long Id { get; set; }

        public string Name { get; set; }

        [DisplayName("Beschreibung")]
        public string Description { get; set; }

        [DisplayName("Ersteller")]
        public string Creator { get; set; }

        [DisplayName("Wortanzahl")]
        public int VocabCount { get; set; }
    }
}
