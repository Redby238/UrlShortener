using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using UrlShortenerRightOne.Models;

namespace UrlShortener.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly SignInManager<IdentityUser> _signInManager;

        public HomeController(ApplicationDbContext context, SignInManager<IdentityUser> signInManager)
        {
            _context = context;
            _signInManager = signInManager;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View("~/Views/Account/Login.cshtml");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, false);
                if (result.Succeeded)
                {
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                    return View("~/Views/Account/Login.cshtml", model);
                }
            }

            return View("~/Views/Account/Login.cshtml", model);
        }
    }
}