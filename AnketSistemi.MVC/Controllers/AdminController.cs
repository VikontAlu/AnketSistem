using Microsoft.AspNetCore.Mvc;

namespace AnketSistemi.MVC.Controllers
{
    public class AdminController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        // YENİ EKLENEN KISIM: Anket Ekleme Sayfası
        [HttpGet]
        public IActionResult CreatePoll()
        {
            return View();
        }
        // YENİ EKLENEN KISIM: Ankete Soru Ekleme Sayfası
        [HttpGet("Admin/AddQuestion/{pollId}")]
        public IActionResult AddQuestion(int pollId)
        {
            ViewBag.PollId = pollId; // Hangi ankete soru eklediğimizi View'a taşıyoruz
            return View();
        }
    }
}