using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyLife.Models;
using MyLife.Repositories;
using MyLife.Services.AccountServices;
using MyLife.Services.TokenGenerators;
using MyLife.Services.TokenValidators;
using MyLife.ViewModels;
using System.Linq;

namespace MyLife.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private IRepository<User> _userRepository;
        private IAccountService _accountService;

        private AccessTokenGenerator _accessTokenGenerator;
        private RefreshTokenGenerator _refreshTokenGenerator;
        private RefreshTokenValidator _refreshTokenValidator;

        public AuthController(
            IRepository<User> userRepository,
            IAccountService accountService,
            AccessTokenGenerator accessTokenGenerator,
            RefreshTokenGenerator refreshTokenGenerator,
            RefreshTokenValidator refreshTokenValidator)
        {
            _userRepository = userRepository;
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

        [Authorize]
        [HttpPost("logout")]
        public IActionResult Logout()
        {
            return Ok();
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
    }
}