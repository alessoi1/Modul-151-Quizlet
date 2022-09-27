using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Quizleter.Data;
using Quizleter.Services.Learnsets;
using Quizleter.ViewModels;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Quizleter.Controllers
{
    public class UsersController : Controller
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ILearnsetService _learnsetService;

        public UsersController(
            SignInManager<IdentityUser>
            signInManager,
            UserManager<IdentityUser> userManager,
            ILearnsetService learnsetService)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _learnsetService = learnsetService;
        }

        public IActionResult Register()
        {
            return View();
        }

        public async Task<IActionResult> Details()
        {
            if (_signInManager.IsSignedIn(User))
            {
                return View(new UserDetailsViewModel
                {
                    Username = User.Identity.Name,
                    Email = User.FindFirst(ClaimTypes.Email).Value,
                    Learnsets = await _learnsetService.GetLearnsetsByUserAsync(User.Identity.Name)
                });
            }
            return Login();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var user = new IdentityUser { UserName = viewModel.Username, Email = viewModel.Email };
                var result = await _userManager.CreateAsync(user, viewModel.Password);
                if (result.Succeeded)
                {
                    await _signInManager.SignInAsync(user, isPersistent: false);
                    return RedirectToAction("Index", "Home");
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            return View();
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(viewModel.Username, viewModel.Password, viewModel.RememberMe, lockoutOnFailure: true);
                if (result.Succeeded)
                {
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                    return View();
                }
            }

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
    }
}
