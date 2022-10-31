using Quizleter.Models;
using System.Collections.Generic;

namespace Quizleter.ViewModels
{
    public class CardViewModel
    {
        public List<Vocab> LeftColumn { get; set; }
        public List<Vocab> MiddleColumn { get; set; }
        public List<Vocab> RightColumn { get; set; }
        public int Rows { get; set; }
    }
}
