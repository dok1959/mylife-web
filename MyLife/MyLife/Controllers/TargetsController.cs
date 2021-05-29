using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyLife.Models;
using MyLife.Models.TargetModels;
using MyLife.Repositories;
using MyLife.ViewModels;
using MyLife.ViewModels.TargetViewModels;
using System.Collections.Generic;
using System.Linq;

namespace MyLife.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TargetsController : ControllerBase
    {
        private IRepository<Target> _targetRepository;
        private IRepository<User> _userRepository;
        public TargetsController(IRepository<Target> targetRepository, IRepository<User> userRepository)
        {
            _targetRepository = targetRepository;
            _userRepository = userRepository;
        }

        [Authorize]
        [HttpGet]
        public IActionResult Get()
        {
            var userId = HttpContext.User.FindFirst("id")?.Value;
            var userTargets = _targetRepository.Find(t => t.Owner.Equals(userId));
            List<TargetViewModel> targets = new List<TargetViewModel>();
            foreach(var target in userTargets)
            {
                targets.Add(new TargetViewModel(target));
            }
            return Ok(targets);
        }

        [Authorize]
        [HttpGet("{id}")]
        public IActionResult Get(string id)
        {
            var userId = HttpContext.User.FindFirst("id")?.Value;
            var target = _targetRepository.GetById(id);
            if(!target.Owner.Equals(userId))
            {
                return BadRequest(new { errorMessage = "You are not owner of this target" });
            }
            return Ok(new TargetViewModel(target));
        }

        [Authorize]
        [HttpPost]
        public IActionResult Post([FromBody] TargetCreationViewModel model)
        {
            var userId = HttpContext.User.FindFirst("id")?.Value;
            var target = new Target(model, userId);
            _targetRepository.Add(target);
            return Ok(new { id = target.Id });
        }

        
        [Authorize]
        [HttpPut]
        public IActionResult Put([FromBody] TargetViewModel model)
        {
            var userId = HttpContext.User.FindFirst("id")?.Value;
            var target = _targetRepository.Find(t => t.Id.Equals(model.Id)).SingleOrDefault();
            if(target == null)
            {
                return BadRequest(new { errorMessage = "Target doesn't exist" });
            }
            target.Title = model.Title;
            _targetRepository.Update(target);
            return Ok();
        }

        
        [Authorize]
        [HttpDelete]
        public IActionResult Delete([FromForm] string id)
        {
            var userId = HttpContext.User.FindFirst("id")?.Value;
            var target = _targetRepository.Find(t => t.Id.Equals(id)).SingleOrDefault();
            if (userId != target.Owner)
            {
                return BadRequest(new { errorMessage = "You are not owner of this target" });
            }
            _targetRepository.Remove(target);
            return Ok();
        }

        [Authorize]
        [HttpPost]
        public IActionResult InviteUser([FromForm] string targetId, string receiverId)
        {
            var userId = HttpContext.User.FindFirst("id")?.Value;
            var target = _targetRepository.Find(t => t.Id.Equals(targetId) || t.Members.Contains(userId)).SingleOrDefault();

            var receiver = _userRepository.Find(u => u.Id.Equals(receiverId)).SingleOrDefault();

            return Ok();
        }

    }
}
