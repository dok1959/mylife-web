using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyLife.Models;
using MyLife.Repositories;
using MyLife.Services.AccountServices;
using MyLife.Services.TokenGenerators;
using MyLife.ViewModels;
using System.Collections.Generic;

namespace MyLife.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private IRepository<User> _repo;
        private IAccountService _accountService;
        private AccessTokenGenerator _accessTokenGenerator;
        private RefreshTokenGenerator _refreshTokenGenerator;

        public AccountController(
            IRepository<User> repo, 
            IAccountService accountService,
            AccessTokenGenerator accessTokenGenerator,
            RefreshTokenGenerator refreshTokenGenerator)
        {
            _repo = repo;
            _accountService = accountService;
            _accessTokenGenerator = accessTokenGenerator;
            _refreshTokenGenerator = refreshTokenGenerator;
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody]LoginViewModel model)
        {
            var user = _accountService.Authenticate(model);
            if (user == null)
            {
                return BadRequest("Wrong user or password!");
            }
            return Ok(new
            {
                accessToken = _accessTokenGenerator.GenerateToken(user),
                refreshToken = _refreshTokenGenerator.GenerateToken()
            });
        }

        [HttpPost("register")]
        public IActionResult Registration([FromBody] RegisterViewModel model)
        {
            _accountService.Register(model);
            return Ok();
        }

        [HttpPost("refresh")]
        public IActionResult Refresh([FromBody] string refreshToken)
        {
            if (refreshToken == null)
                return BadRequest("Wrong refresh token");

            return Ok();
        }

        [HttpPost("update")]
        public IActionResult Update()
        {
            return Ok();
        }

        [Authorize]
        [HttpGet("getallusers")]
        public IActionResult GetAllUsers()
        {
            var users = new List<UserViewModel>();
            foreach (var user in _repo.GetAll())
            {
                users.Add(new UserViewModel(user));
            }
            return Ok(users);
        }

        [Authorize]
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
    }
}