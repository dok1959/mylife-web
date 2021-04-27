using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyLife.Models;
using MyLife.Repositories;
using MyLife.ViewModels;
using System.Linq;

namespace MyLife.Controllers
{
    public class DesiresController : ControllerBase
    {
        private IRepository<Desire> _desireRepository;
        private IMapper _mapper;
        public DesiresController(IRepository<Desire> desireRepository, IMapper mapper)
        {
            _desireRepository = desireRepository;
            _mapper = mapper;
        }

        //GET api/desires
        [Authorize]
        [HttpGet]
        public IActionResult GetAllDesires()
        {
            var userId = HttpContext.User.FindFirst("id")?.Value;
            var repoDesires = _desireRepository.Find(desire => desire.Owner == userId);
            if(repoDesires == null)
            {
                return BadRequest(new { errorMessage = "You have no desires" });
            }
            var desires = _mapper.Map<DesireViewModel>(repoDesires);
            return Ok(desires);
        }

        //GET api/desires/id
        [Authorize]
        [HttpGet("{id}")]
        public IActionResult GetDesireById(string id)
        {
            var userId = HttpContext.User.FindFirst("id")?.Value;
            var desire = _desireRepository.GetById(id);

            if(desire == null)
            {
                return BadRequest(new { errorMessage = "There is no desire with this id" });
            }    
            if(desire.Owner != userId)
            {
                return BadRequest(new { errorMessage = "You have no desire with this id" });
            }

            return Ok(desire);
        }

        //POST api/desires
        [Authorize]
        [HttpPost]
        public IActionResult CreateDesire([FromBody] DesireViewModel model)
        {
            var desire = new Desire(model);
            var userId = HttpContext.User.FindFirst("id")?.Value;

            desire.Owner = userId;
            desire.Members.Add(userId);

            //var subDesires = model.SubDesires.Select(x => new SubDesire { Title = x.Title, UserId = userId, MaxValue = x.MaxValue, CurrentValue = x.CurrentValue, Order = x.Order});
            /*foreach (var subDesireViewModel in model.SubDesires)
            {
                var subDesire = new SubDesire(subDesireViewModel);
                subDesire.UserId = userId;
                subDesires.Add(subDesire);
            }
            desire.SubDesires = subDesires;*/

            /*var subDesires = (from DesireViewModel in model
                             select new Desire { Title = model.Title });*/

            _desireRepository.Add(desire);
            return Ok();
        }

        //POST api/desires/id/invitations
        [Authorize]
        [HttpPost("{id}/invitations")]
        public IActionResult InviteMember(string id, [FromBody] string memberId)
        {
            return Ok();
        }

        //PUT api/desires
        [Authorize]
        [HttpPut]
        public IActionResult UpdateDesire([FromBody] Desire model)
        {
            var desire = _desireRepository.GetById(model.Id);

            if(desire == null)
            {
                return BadRequest(new { errorMessage = "Desire doesn't exist" });
            }

            var userId = HttpContext.User.FindFirst("id")?.Value;

            if (desire.Owner != userId)
            {
                return BadRequest(new { errorMessage = "You are not the owner of the desire" });
            }
            _desireRepository.Update(model);
            return Ok();
        }

        //DELETE api/desires
        [Authorize]
        [HttpDelete("{id}")]
        public IActionResult DeleteDesire(string id)
        {
            var userId = HttpContext.User.FindFirst("id")?.Value;

            var desire = _desireRepository.GetById(id);

            if(desire == null)
            {
                return BadRequest(new { errorMessage = "Desire doesn't exist" });
            }

            if(desire.Owner != userId)
            {
                return BadRequest(new { errorMessage = "You are not the owner of the desire" });
            }

            _desireRepository.Remove(desire);
            return Ok();
        }
    }
}
