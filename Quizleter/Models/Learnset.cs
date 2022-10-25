using System.ComponentModel;

namespace Quizleter.Models
{
    public class Learnset
    {
        public long Id { get; set; }

        public string Name { get; set; }

        [DisplayName("Beschreibung")]
        public string Desc { get; set; }

        [DisplayName("Ersteller")]
        public string CreatorUsername { get; set; }
    }
}
