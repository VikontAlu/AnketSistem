using Microsoft.AspNetCore.Mvc;

namespace AnketSistemi.MVC.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        // YENÝ EKLENEN KISIM: Týklanan anketin sayfasýna gitmek için
        [HttpGet("Home/PollDetail/{id}")]
        public IActionResult PollDetail(int id)
        {
            ViewBag.PollId = id; // ID'yi AJAX kodunda kullanmak için View'e taþýyoruz
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }
    }
}