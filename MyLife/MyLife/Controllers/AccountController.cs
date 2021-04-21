using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using MyLife.Models;
using MyLife.Repositories;
using MyLife.Services;
using MyLife.ViewModels;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace MyLife.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private IRepository<User> _repo;
        private IAccountService _accountService;
        private IConfiguration _config;
        public AccountController(IRepository<User> repo, IAccountService accountService, IConfiguration config)
        {
            _repo = repo;
            _accountService = accountService;
            _config = config;
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody]LoginViewModel model)
        {
            var user = _accountService.Authenticate(model);
            if (user == null)
            {
                return BadRequest("Wrong user or password!");
            }
            var userClaims = GetClaims(user);
            var accessConfig = new AuthenticationConfiguration
            {

            }
            var access_token = CreateJwtToken(userClaims);
            var refresh_token = CreateJwtToken(userClaims);
            return Ok(new
            {
                access_token = access_token,
                refresh_token = refresh_token
            });
        }

        [HttpPost("register")]
        public IActionResult Registration([FromBody] RegisterViewModel model)
        {
            _accountService.Register(model);
            return Ok();
        }

        [HttpPost("update")]
        public IActionResult Update()
        {
            return Ok();
        }

        //[Authorize]
        [HttpGet("getallusers")]
        public IActionResult GetAllUsers()
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
            return Ok(users);
        }

        [HttpGet("getuser/{id}")]
        public IActionResult GetUser(string id)
        {
            var user = _repo.Get(id);
            if(user == null)
                return NotFound("User not found");

            return Ok(new
            {
                id = user.Id.ToString(),
                login = user.Login,
                username = user.Username,
                firstName = user.FirstName,
                lastName = user.LastName,
                city = user.City,
                email = user.Email
            });
        }

        /*[HttpPost("adduser")]
        public void AddUser([FromBody] User user)
        {
            _repo.Add(user);
        }*/

        [NonAction]
        private ClaimsIdentity GetClaims(User user)
        {
            if (user != null)
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimsIdentity.DefaultNameClaimType, user.Login),
                };
                ClaimsIdentity claimsIdentity = new ClaimsIdentity(
                    claims, "Token", 
                    ClaimsIdentity.DefaultNameClaimType, 
                    "User");
                return claimsIdentity;
            }
            return null;
        }

        [NonAction]
        private string CreateJwtToken(ClaimsIdentity identity, AuthenticationConfiguration authConfig)
        {
            var now = DateTime.UtcNow;
            var jwt = new JwtSecurityToken(
                    issuer: authConfig.Issuer,
                    audience: authConfig.Audience,
                    notBefore: now,
                    claims: identity.Claims,
                    expires: now.Add(TimeSpan.FromMinutes(authConfig.TokenExpirationMinutes)),
                    signingCredentials: new SigningCredentials(authConfig.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256));
            return new JwtSecurityTokenHandler().WriteToken(jwt);
        }
    }
}