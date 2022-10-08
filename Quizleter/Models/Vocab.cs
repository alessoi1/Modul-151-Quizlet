namespace Quizleter.Models
{
    public class Vocab
    {
        public long Id { get; set; }

        public string Definition { get; set; }

        public string Term { get; set; }


        public long LearnsetId { get; set; }

        public Learnset Learnset { get; set; }
    }
}
