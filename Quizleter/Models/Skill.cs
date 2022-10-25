namespace Quizleter.Models
{
    public class Skill
    {
        public string Username { get; set; }
        public long VocabId { get; set; }
        public Vocab Vocab { get; set; }
        public long SkillLevel { get; set; }
    }
}