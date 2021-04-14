using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using MongoDB.Bson;
using MyLife.Data;
using MyLife.Models;
using MyLife.Repositories;
using MyLife.ViewModels;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;

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

        [HttpGet("login")]
        public IActionResult Login([FromQuery]LoginViewModel model)
        {
            var identity = GetIdentity(model.Login, model.Password);
            if(identity == null)
            {
                return BadRequest(new { errorText = "Invalid login or password" });
            }
            return CreateJwtToken(identity);
        }

        [HttpPost("register")]
        public IActionResult Registration([FromBody] RegisterViewModel model)
        {
            return Ok();
        }

        [HttpPost("update")]
        public IActionResult Update()
        {
            return Ok();
        }

        //[Authorize]
        [HttpGet("getallusers")]
        public JsonResult GetAllUsers()
        {
            //_repo.Add(new User { Login = "Genadiy", Nickname = "Genchik", Password = "WHAAAt" });
            var users = new List<UserViewModel>();
            foreach (var user in _repo.GetAll())
            {
                users.Add(new UserViewModel
                {
                    Id = user.Id.ToString(),
                    Username = user.Username
                });
            }
            return new JsonResult(users);
        }

        [HttpGet("getuser/{id}")]
        public IActionResult GetUser(string id)
        {
            var objectId = ObjectId.Parse(id);
            if(objectId == null)
                return NotFound("Wrong user id");

            var user = _repo.Get(objectId);
            if(user == null)
                return NotFound("User not found");

            return new JsonResult(user);
        }

        [NonAction]
        private ClaimsIdentity GetIdentity(string login, string password)
        {
            var user = _repo.Find(x => x.Login == login && x.Password == password).FirstOrDefault();
            if (user != null)
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimsIdentity.DefaultNameClaimType, user.Login),
                };
                ClaimsIdentity claimsIdentity = new ClaimsIdentity(
                    claims, "Token", 
                    ClaimsIdentity.DefaultNameClaimType, 
                    ClaimsIdentity.DefaultRoleClaimType);
                return claimsIdentity;
            }
            return null;
        }

        [NonAction]
        private JsonResult CreateJwtToken(ClaimsIdentity identity)
        {
            var now = DateTime.UtcNow;
            var jwt = new JwtSecurityToken(
                    issuer: AuthOptions.ISSUER,
                    audience: AuthOptions.AUDIENCE,
                    notBefore: now,
                    claims: identity.Claims,
                    expires: now.Add(TimeSpan.FromMinutes(AuthOptions.LIFETIME)),
                    signingCredentials: new SigningCredentials(AuthOptions.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256));
            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);
            return new JsonResult(new { access_token = encodedJwt } );
        }
    }
}
