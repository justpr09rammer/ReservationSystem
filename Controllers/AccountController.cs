using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using ReservationSystem.Models.Entities;
using ReservationSystem.Models.ViewModels;
using ReservationSystem.Services;
using System.Threading.Tasks;

namespace ReservationSystem.Controllers
{
    public class AccountController : Controller
    {
        private readonly IAuthService _authService;
        private readonly ILogService _logService;

        public AccountController(IAuthService authService, ILogService logService)
        {
            _authService = authService;
            _logService = logService;
        }

        [HttpGet]
        public IActionResult Login()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Reservation");
            }
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var barber = await _authService.AuthenticateAsync(model.Username, model.Password);
                    if (barber != null)
                    {
                        await _authService.SignInAsync(barber);
                        return RedirectToAction("Index", "Reservation");
                    }
                    ModelState.AddModelError("", "İstifadəçi adı və ya şifrə yanlışdır");
                }
                catch (System.Exception ex)
                {
                    await _logService.LogExceptionAsync(ex, nameof(Login));
                    ModelState.AddModelError("", "Giriş zamanı xəta baş verdi");
                }
            }
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await _authService.SignOutAsync();
            return RedirectToAction("Login");
        }
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(Barber barber)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var success = await _authService.RegisterAsync(barber);
                    if (success)
                    {
                        await _authService.SignInAsync(barber);
                        return RedirectToAction("Index", "Reservation");
                    }
                    ModelState.AddModelError("", "Bu istifadəçi adı artıq mövcuddur.");
                }
                catch (Exception ex)
                {
                    await _logService.LogExceptionAsync(ex, nameof(Register));
                    ModelState.AddModelError("", "Qeydiyyat zamanı xəta baş verdi.");
                }
            }

            return View(barber);
        }

    }
}
