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
        public IActionResult AddQuestion(int pollId, int? questionId = null)
        {
            ViewBag.PollId = pollId;
            ViewBag.QuestionId = questionId;
            ViewBag.ApiBaseUrl = _config["ApiBaseUrl"];
            return View();
        }
        [HttpGet("Admin/ViewResponses")]
        public IActionResult ViewResponses()
        {
            ViewBag.ApiBaseUrl = _config["ApiBaseUrl"];
            return View();
        }

        [HttpGet("Admin/EditResponse/{id}")]
        public IActionResult EditResponse(int id)
        {
            ViewBag.ResponseId = id;
            ViewBag.ApiBaseUrl = _config["ApiBaseUrl"];
            return View();
        }
        [HttpGet("Admin/EditPoll/{id}")]
        public IActionResult EditPoll(int id)
        {
            ViewBag.PollId = id;
            ViewBag.ApiBaseUrl = _config["ApiBaseUrl"];
            return View();
        }
        [HttpGet("Admin/ViewFeedbacks")]
        public IActionResult ViewFeedbacks()
        {
            ViewBag.ApiBaseUrl = _config["ApiBaseUrl"];
            return View();
        }
        [HttpGet("Admin/Results/{id?}")]  
        public IActionResult Results(int? id)
        {
            ViewBag.ApiBaseUrl = _config["ApiBaseUrl"];
            if (id.HasValue)
            {
              
                ViewBag.PollId = id.Value;
                return View("Results");
            }
            else
            {
              
                return View("SelectPollForResults");
            }
        }



    }
}
