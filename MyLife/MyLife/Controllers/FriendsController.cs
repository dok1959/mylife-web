using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MyLife.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FriendsController : ControllerBase
    {
        [Authorize]
        [HttpGet]
        public IActionResult Get()
        {
            return Ok();
        }

        [Authorize]
        [HttpGet("{id}")]
        public IActionResult Get(string id)
        {
            return Ok();
        }

        [Authorize]
        [HttpPost("{id}")]
        public IActionResult Add([FromBody] string id)
        {
            return Ok();
        }

        [Authorize]
        [HttpDelete("{id}")]
        public IActionResult Delete([FromBody] string id)
        {
            return Ok();
        }

        [Authorize]
        [HttpGet("find")]
        public IActionResult FindAll()
        {
            return Ok();
        }

        [Authorize]
        [HttpGet("find/{username}")]
        public IActionResult FindByUsername(string username)
        {
            return Ok();
        }
    }
}
