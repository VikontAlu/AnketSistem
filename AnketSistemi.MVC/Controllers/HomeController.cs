using AnketSistemi.MVC.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace AnketSistemi.MVC.Controllers
{
    public class HomeController : Controller
    {
        private readonly IConfiguration _config;

        public HomeController(IConfiguration config)
        {
            _config = config;
        }

        public IActionResult Index()
        {
            ViewBag.ApiBaseUrl = _config["ApiBaseUrl"];
            return View();
        }

        public IActionResult PollDetail(int id)
        {
            ViewBag.PollId = id;
            ViewBag.ApiBaseUrl = _config["ApiBaseUrl"];
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
