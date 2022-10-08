using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

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
