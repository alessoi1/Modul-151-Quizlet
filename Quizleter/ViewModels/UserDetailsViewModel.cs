using Quizleter.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Quizleter.ViewModels
{
    public class UserDetailsViewModel
    {
        public string Email { get; set; }

        public IEnumerable<Learnset> Learnsets { get; set; }
    }
}