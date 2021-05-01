using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyLife.Models;
using MyLife.Repositories;
using MyLife.ViewModels;

namespace MyLife.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProfileController : ControllerBase
    {
        IRepository<User> _usersRepository;
        IMapper _mapper;

        public ProfileController(IRepository<User> usersRepository, IMapper mapper)
        {
            _usersRepository = usersRepository;
            _mapper = mapper;
        }

        [Authorize]
        [HttpGet]
        public IActionResult Get()
        {
            var userId = HttpContext.User.FindFirst("id")?.Value;
            var repoUser = _usersRepository.GetById(userId);
            if(repoUser == null)
            {
                BadRequest("User with this id doesn't exist");
            }
            return Ok(_mapper.Map<ProfileViewModel>(repoUser));
        }


        [Authorize]
        [HttpPatch]
        public IActionResult Update([FromBody] MyProfileViewModel model)
        {
            var userId = HttpContext.User.FindFirst("id")?.Value;
            var repoUser = _usersRepository.GetById(userId);
            if (repoUser == null)
            {
                BadRequest("User with this id doesn't exist");
            }

            /* Need improve */
            repoUser.Login = model.Login;
            repoUser.Username = model.Username;
            repoUser.FirstName = model.FirstName;
            repoUser.LastName = model.LastName;
            repoUser.Email = model.Email;
            repoUser.City = model.City;
            /*-------------*/

            _usersRepository.Update(repoUser);
            return Ok();
        }
    }
}
