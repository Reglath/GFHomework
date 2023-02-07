using Homework.Models.DTOs;
using Homework.Models.Entities;
using Homework.Services;
using Microsoft.AspNetCore.Mvc;

namespace Homework.Controllers
{
    [Route("")]
    [ApiController]
    public class HomeController : Controller
    {
        private UserService userService { get; set; }

        public HomeController(UserService userService)
        {
            this.userService = userService;
        }

        [HttpPost("/register")]
        public IActionResult Register([FromBody]RegisterDTO registerDTO)
        {
            var result = userService.Register(registerDTO);
            return StatusCode(result.Status, result.Message);
        }

        [HttpPost("/login")]
        public IActionResult Login([FromBody] LoginDTO loginDTO)
        {
            var result = userService.Login(loginDTO);
            return StatusCode(result.Status, result.Message);
        }
    }
}
