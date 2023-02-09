using Homework.Models.DTOs;
using Homework.Models.Entities;
using Homework.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Homework.Controllers
{
    [Route("")]
    [ApiController]
    [Authorize]
    public class HomeController : Controller
    {
        private IUserService userService { get; set; }
        private IItemService itemService { get; set; }

        public HomeController(IUserService userService, IItemService itemService)
        {
            this.userService = userService;
            this.itemService = itemService;
        }

        [HttpPost("/register"), AllowAnonymous]
        public IActionResult Register([FromBody]RegisterDTO registerDTO)
        {
            var result = userService.Register(registerDTO);
            return StatusCode(result.Status, result.Message);
        }

        [HttpPost("/login"), AllowAnonymous]
        public IActionResult Login([FromBody] LoginDTO loginDTO)
        {
            var result = userService.Login(loginDTO);
            return StatusCode(result.Status, result.Message);
        }

        [HttpPost("/getJWT"), AllowAnonymous]
        public IActionResult GetJWT([FromBody] LoginDTO loginDTO)
        {
            var result = userService.GetJWT(loginDTO);
            return StatusCode(result.Status, result.Message);
        }

        [HttpPost("/sell")]
        public IActionResult Sell([FromBody] SellDTO sellDTO)
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            var request = new SellRequestDTO() { SellDTO = sellDTO, Username = identity.FindFirst("Username").Value };
            var result = itemService.Sell(request);

            if (result.Status == 400)
                return StatusCode(result.Status, result.Message);

            var viewDTO = new SellViewDTO(result.Item);
            return StatusCode(result.Status, viewDTO);
        }

        [HttpGet("/list/{n}")]
        public IActionResult List(int n)
        {
            var result = itemService.List(n);
            if (result.Status == 400)
                return StatusCode(result.Status, result.Message);

            return StatusCode(result.Status, result.Items);
        }

        [HttpGet("/view/{id}")]
        public IActionResult ViewSpecific(int id)
        {
            var result = itemService.ViewSpecific(id);
            if (result.Status == 400)
                return StatusCode(result.Status, result.Message);

            return StatusCode(result.Status, result.View);
        }

        [HttpPost("/bid")]
        public IActionResult Bid(BidDTO bidDTO)
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            var request = new BidRequestDTO() { Username = identity.FindFirst("Username").Value, BidDTO = bidDTO };
            var result = itemService.Bid(request);

            if (result.Status == 400)
                return StatusCode(result.Status, result.Message);

            return StatusCode(result.Status, result.Item);
        }
    }
}
