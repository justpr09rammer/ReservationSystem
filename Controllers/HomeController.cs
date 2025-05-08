using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using ReservationSystem.Models;
using ReservationSystem.Services;

namespace ReservationSystem.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ILogService _logService;

        public HomeController(ILogger<HomeController> logger, ILogService logService)
        {
            _logger = logger;
            _logService = logService;
        }

        public IActionResult Index()
        {

            return RedirectToAction("Create", "Reservation");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public async Task<IActionResult> Error()
        {
            var exceptionHandlerPathFeature = HttpContext.Features.Get<Microsoft.AspNetCore.Diagnostics.IExceptionHandlerPathFeature>();
            
            if (exceptionHandlerPathFeature?.Error != null)
            {
                await _logService.LogExceptionAsync(exceptionHandlerPathFeature.Error);
            }
            
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
