using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyLife.Models;
using MyLife.Models.TargetModels;
using MyLife.Repositories;
using MyLife.Services.TargetServices;
using MyLife.ViewModels;
using MyLife.ViewModels.TargetViewModels;
using System;
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
        private ITargetService _targetService;
        public TargetsController(IRepository<Target> targetRepository, IRepository<User> userRepository, ITargetService targetService)
        {
            _targetRepository = targetRepository;
            _userRepository = userRepository;
            _targetService = targetService;
        }

        [Authorize]
        [HttpGet]
        public IActionResult Get([FromForm] string date)
        {
            var userId = HttpContext.User.FindFirst("id")?.Value;
            var userTargets = _targetRepository.Find(t => (t.Owner.Equals(userId) || t.Members.Equals(userId))).ToList();

            if(date == null || !DateTime.TryParse(date, out DateTime currentDate))
                return BadRequest(new { errorMessage = "Wrong Date" });

            List<TargetViewModel> targets = new List<TargetViewModel>();
            foreach(var target in userTargets)
            {
                var targetWithCurrentDate = target.Progress.Find(t => t.Date.Value.Equals(currentDate.Date));
                if (targetWithCurrentDate != null)
                {
                    target.Progress = new List<Progress>();
                    target.Progress.Add(targetWithCurrentDate);
                }
                targets.Add(new TargetViewModel(target));
            }
            return Ok(targets);
        }

        [Authorize]
        [HttpGet("{id}")]
        public IActionResult Get(string id, [FromForm] string date)
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
        [HttpPost("invitations")]
        public IActionResult InviteUser([FromForm] string targetId, [FromForm] string receiverId)
        {
            var userId = HttpContext.User.FindFirst("id")?.Value;

            var receiver = _userRepository.Find(u => u.Id.Equals(receiverId)).SingleOrDefault();
            if(receiver == null)
            {
                return BadRequest(new { errorMessage = "User doesn't exist" });
            }

            var target = _targetRepository.Find(t => t.Owner.Equals(receiverId) || t.Members.Contains(receiverId)).SingleOrDefault();
            if(target != null)
            {
                return BadRequest(new { errorMessage = "User already involved in this target" });
            }

            var targetInvitation = receiver.TargetsInvitations.Find(t => t.SenderId.Equals(userId) && t.TargetId.Equals(targetId));
            if (targetInvitation != null)
            {
                return BadRequest(new { errorMessage = "Invitation to this target already exist" });
            }

            _targetService.InviteUser(receiver, userId, targetId);

            return Ok();
        }

        [Authorize]
        [HttpDelete("invitations")]
        public IActionResult DeleteUser([FromForm] string targetId, [FromForm] string receiverId)
        {
            var userId = HttpContext.User.FindFirst("id")?.Value;
            var target = _targetRepository.Find(t => t.Id.Equals(targetId) && (t.Members.Contains(userId) || t.Owner.Equals(userId))).SingleOrDefault();

            var receiver = _userRepository.Find(u => u.Id.Equals(receiverId)).SingleOrDefault();
            if (receiver == null)
            {
                return BadRequest(new { errorMessage = "User doesn't exist" });
            }
            _targetService.DeleteUser(receiver, userId, target);

            return Ok();
        }

        [Authorize]
        [HttpPost("invitations")]
        public IActionResult AcceptInvitation([FromForm] string targetId)
        {
            var userId = HttpContext.User.FindFirst("id")?.Value;

            var receiver = _userRepository.Find(u => u.Id.Equals(userId)).SingleOrDefault();
            if (receiver == null)
            {
                return BadRequest(new { errorMessage = "User doesn't exist" });
            }

            var target = _targetRepository.Find(t => t.Id.Equals(targetId)).SingleOrDefault();
            if(target == null)
            {
                return BadRequest(new { errorMessage = "Target with this id doesn't exist" });
            }

            if(target.Owner.Equals(userId) || target.Members.Contains(userId))
            {
                return BadRequest(new { errorMessage = "User already involved in this target" });
            }

            var targetInvitation = receiver.TargetsInvitations.Find(t => t.TargetId.Equals(targetId));
            if(targetInvitation == null)
            {
                return BadRequest(new { errorMessage = "User doesn't received invitation to target with this id" });
            }

            _targetService.AcceptInvitation(receiver, targetInvitation, target);

            return Ok();
        }

        [Authorize]
        [HttpPost("invitations")]
        public IActionResult DeclineInvitation([FromForm] string targetId)
        {
            var userId = HttpContext.User.FindFirst("id")?.Value;

            var receiver = _userRepository.Find(u => u.Id.Equals(userId)).SingleOrDefault();
            if (receiver == null)
            {
                return BadRequest(new { errorMessage = "User doesn't exist" });
            }

            var targetInvitation = receiver.TargetsInvitations.Find(t => t.TargetId.Equals(targetId));
            if (targetInvitation == null)
            {
                return BadRequest(new { errorMessage = "User doesn't received invitation to target with this id" });
            }

            _targetService.DeclineInvitation(receiver, targetInvitation);
            return Ok();
        }

    }
}
