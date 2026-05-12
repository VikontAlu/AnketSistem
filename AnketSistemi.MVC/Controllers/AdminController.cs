using Microsoft.AspNetCore.Mvc;

namespace AnketSistemi.MVC.Controllers
{
    public class AdminController : Controller
    {
        private readonly IConfiguration _config;

        public AdminController(IConfiguration config)
        {
            _config = config;
        }

        public IActionResult Index()
        {
            ViewBag.ApiBaseUrl = _config["ApiBaseUrl"];
            return View();
        }

        [HttpGet]
        public IActionResult CreatePoll()
        {
            ViewBag.ApiBaseUrl = _config["ApiBaseUrl"];
            return View();
        }

        [HttpGet("Admin/AddQuestion/{pollId}")]
        public IActionResult AddQuestion(int pollId)
        {
            ViewBag.PollId = pollId;
            ViewBag.ApiBaseUrl = _config["ApiBaseUrl"];
            return View();
        }
    }
}
