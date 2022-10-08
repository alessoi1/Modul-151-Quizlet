using Quizleter.Models;
using System.Collections.Generic;

namespace Quizleter.ViewModels
{
    public class UserDetailsViewModel
    {
        public string Username { get; set; }

        public string Email { get; set; }

        public IEnumerable<Learnset> Learnsets { get; set; }
    }
}