using Homework.Services;
using Microsoft.AspNetCore.Mvc;

namespace Homework.Controllers
{
    [Route("")]
    public class HomeController : Controller
    {
        private Service service { get; set; }

        public HomeController(Service service)
        {
            this.service = service;
        }

        public IActionResult Index()
        {
            HttpContext.Session.SetString("user", "x");
            //vm.User = HttpContext.Session.GetString("user");
            return View();
        }
    }
}
