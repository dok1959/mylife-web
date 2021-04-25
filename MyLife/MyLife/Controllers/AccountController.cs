using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyLife.Models;
using MyLife.Repositories;
using MyLife.Services.AccountServices;
using MyLife.Services.TokenGenerators;
using MyLife.Services.TokenValidators;
using MyLife.ViewModels;
using System.Collections.Generic;
using System.Linq;

namespace MyLife.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private IRepository<User> _userRepository;
        private IRepository<Desire> _desireRepository;
        private IAccountService _accountService;

        private AccessTokenGenerator _accessTokenGenerator;
        private RefreshTokenGenerator _refreshTokenGenerator;
        private RefreshTokenValidator _refreshTokenValidator;

        public AccountController(
            IRepository<User> userRepository,
            IRepository<Desire> desireRepository,
            IAccountService accountService,
            AccessTokenGenerator accessTokenGenerator,
            RefreshTokenGenerator refreshTokenGenerator,
            RefreshTokenValidator refreshTokenValidator)
        {
            _userRepository = userRepository;
            _desireRepository = desireRepository;
            _accountService = accountService;
            _accessTokenGenerator = accessTokenGenerator;
            _refreshTokenGenerator = refreshTokenGenerator;
            _refreshTokenValidator = refreshTokenValidator;
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody]LoginViewModel model)
        {
            var user = _accountService.Authenticate(model);
            if (user == null)
            {
                return BadRequest(new { errorMessage = "Wrong login or password" });
            }

            user.RefreshToken = _refreshTokenGenerator.GenerateToken();
            _userRepository.Update(user);

            return Ok(new
            {
                accessToken = _accessTokenGenerator.GenerateToken(user),
                refreshToken = user.RefreshToken
            });
        }

        [HttpPost("register")]
        public IActionResult Registration([FromBody] RegisterViewModel model)
        {
            if (_userRepository.Find(u => u.Login == model.Login).FirstOrDefault() != null)
            {
                return BadRequest(new { errorMessage = "User with this login is already registered" });
            }
            _accountService.Register(model);
            return Ok();
        }

        [HttpPost("refresh")]
        public IActionResult Refresh([FromBody] RefreshTokenViewModel model)
        {
            if (model.RefreshToken == null)
                return BadRequest("Wrong refresh token");

            bool isRefreshTokenValid = _refreshTokenValidator.Validate(model.RefreshToken);
            if(!isRefreshTokenValid)
            {
                return BadRequest(new { errorMessage = "Invalid refresh token" });
            }

            var user = _userRepository.FindOne(u => u.RefreshToken == model.RefreshToken);

            if(user == null)
            {
                return BadRequest(new { errorMessage = "User with this token not found" });
            }

            user.RefreshToken = _refreshTokenGenerator.GenerateToken();
            _userRepository.Update(user);

            return Ok(new
            {
                accessToken = _accessTokenGenerator.GenerateToken(user),
                refreshToken = user.RefreshToken
            });
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
            foreach (var user in _userRepository.GetAll())
            {
                users.Add(new UserViewModel(user));
            }
            return Ok(users);
        }

        [Authorize]
        [HttpGet("getuser/{id}")]
        public IActionResult GetUser(string id)
        {
            var user = _userRepository.GetById(id);
            if(user == null)
                return NotFound("User not found");

            return Ok(new
            {
                id = user.Id,
                login = user.Login,
                username = user.Username,
                firstName = user.FirstName,
                lastName = user.LastName,
                city = user.City,
                email = user.Email
            });
        }

        [Authorize]
        [HttpPost("adddesire")]
        public IActionResult AddDesire([FromBody] DesireViewModel model)
        {
            var desire = new Desire(model);
            var userId = HttpContext.User.FindFirst("id")?.Value;

            desire.Owner = userId;
            desire.Members.Add(userId);

            var subDesires = new List<SubDesire>();
            foreach(var subDesireViewModel in model.SubDesires)
            {
                var subDesire = new SubDesire(subDesireViewModel);
                subDesire.UserId = userId;
                subDesires.Add(subDesire);
            }

            desire.SubDesires = subDesires;

            _desireRepository.Add(desire);
            return Ok();
        }
    }
}