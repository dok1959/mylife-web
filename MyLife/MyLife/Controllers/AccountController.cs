using Microsoft.AspNetCore.Mvc;
using MyLife.Models;
using System.Security.Claims;

namespace MyLife.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        ApplicationContext _context;
        public AccountController(ApplicationContext context)
        {
            _context = context;
        }

        [HttpPost]
        public IActionResult Login([FromBody] LoginViewModel model)
        {
            var identity = GetIdentity(model.Login, model.Password);
            if(identity == null)
            {
                return BadRequest(new { errorText = "Invalid login or password" });
            }
            return CreateJwtToken(identity);
        }

        [HttpPost]
        [ActionName("register")]
        public IActionResult Registration([FromBody] RegisterViewModel model)
        {
            return Ok();
        }

        [HttpPost]
        public IActionResult UpdateAccount()
        {
            return Ok();
        }

        [NonAction]
        private ClaimsIdentity GetIdentity(string login, string password)
        {
            return null;
        }

        [NonAction]
        private JsonResult CreateJwtToken(ClaimsIdentity identity)
        {
            return null;
        }
    }
}
