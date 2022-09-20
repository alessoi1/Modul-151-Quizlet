using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Quizleter.Models;

namespace Quizleter.Data
{
    public class AuthContext : IdentityDbContext<IdentityUser, IdentityRole, string>
    {
        public AuthContext (DbContextOptions<AuthContext> options)
            : base(options)
        {
        }
    }
}
