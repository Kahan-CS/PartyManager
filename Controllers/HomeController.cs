using Microsoft.AspNetCore.Mvc;
using PartyManager.Models;
using System.Diagnostics;

namespace PartyManager.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            const string cookieKey = "FirstVisit";

            if (!Request.Cookies.ContainsKey(cookieKey))
            {
                var options = new CookieOptions { Expires = DateTime.Now.AddYears(30) };
                Response.Cookies.Append(cookieKey, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), options);
                ViewData["WelcomeMessage"] = "Welcome to the Party Guest Manager App!";
            }
            else
            {
                string firstVisitDate = Request.Cookies[cookieKey]!;
                ViewData["WelcomeMessage"] = $"Welcome back! You first visited this app on {firstVisitDate}.";
            }

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
