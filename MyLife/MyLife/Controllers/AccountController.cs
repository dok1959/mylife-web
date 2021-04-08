using Microsoft.AspNetCore.Mvc;
using MyLife.Data;
using MyLife.Models;
using MyLife.Repositories;
using System.Security.Claims;
using System.Text;

namespace MyLife.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private UserRepository _repo;
        public AccountController()
        {
            _repo = new UserRepository(new ApplicationContext());
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
        public IActionResult Update()
        {
            return Ok();
        }

        [HttpGet("getusers")]
        public string GetAllUsers()
        {
            var repoUsers = _repo.GetAll();
            StringBuilder builder = new StringBuilder();
            foreach(var user in repoUsers)
                builder.Append(user.Login + " " + user.Password);
            return builder.ToString();
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
