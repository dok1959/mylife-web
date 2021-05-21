using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyLife.Models.TargetModels;
using MyLife.Repositories;
using MyLife.ViewModels;
using MyLife.ViewModels.TargetViewModels;
using System.Collections.Generic;

namespace MyLife.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TargetsController : ControllerBase
    {
        private IRepository<Target> _targetRepository;
        public TargetsController(IRepository<Target> targetRepository)
        {
            _targetRepository = targetRepository;
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
            return Ok();
        }

        
        [Authorize]
        [HttpPut]
        public IActionResult Put([FromBody] TargetViewModel model)
        {
            var userId = HttpContext.User.FindFirst("id")?.Value;
            var target = _targetRepository.GetById(model.Id);
            target.Title = model.Title;
            _targetRepository.Update(target);
            return Ok();
        }

        /*
        [Authorize]
        [HttpDelete]
        public IActionResult Delete([FromForm] string id)
        {
            var userId = HttpContext.User.FindFirst("id")?.Value;
            var target = _targetRepository.GetById(id);
            if (userId != target.Owner)
            {
                return BadRequest(new { errorMessage = "You are not owner of this target" });
            }
            _targetRepository.Remove(target);
            return Ok();
        }
        */
    }
}
