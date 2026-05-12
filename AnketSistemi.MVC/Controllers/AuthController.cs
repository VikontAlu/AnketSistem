using Microsoft.AspNetCore.Mvc;

namespace AnketSistemi.MVC.Controllers
{
    public class AuthController : Controller
    {
        private readonly IConfiguration _config;

        public AuthController(IConfiguration config)
        {
            _config = config;
        }

        [HttpGet]
        public IActionResult Login()
        {
            ViewBag.ApiBaseUrl = _config["ApiBaseUrl"];
            return View();
        }

        [HttpGet]
        public IActionResult Register()
        {
            ViewBag.ApiBaseUrl = _config["ApiBaseUrl"];
            return View();
        }
        [HttpGet]
        public IActionResult Profile()
        {
            ViewBag.ApiBaseUrl = _config["ApiBaseUrl"];
            return View();
        }
        [HttpGet]
        public IActionResult ForgotPassword()
        {
            return View();
        }

        [HttpGet]
        public IActionResult ResetPassword(string email, string token)
        {
            ViewBag.Email = email;
            ViewBag.Token = token;
            return View();
        }
    }
}
    